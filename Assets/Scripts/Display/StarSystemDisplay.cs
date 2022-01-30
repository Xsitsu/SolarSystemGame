using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSystemDisplay : MonoBehaviour
{
    public GameObject nonePrefab;
    public GameObject starPrefab;
    public GameObject planetPrefab;
    public GameObject stationPrefab;

    public Orbital anchor;

    public double timeFactor = 1.0;

    Star _star;
    Hashtable orbitalMap = new Hashtable();

    static private StarSystemDisplay _instance;
	static public StarSystemDisplay Instance { get { return _instance; } }
    void Awake()
    {
        _instance = this;
    }
    void OnDestroy()
    {
        _instance = null;
    }

    void Start()
    {
        LoadStarSystem(new SystemGeneratorSol().Generate());
    }

    void Update()
    {
        Vector3d worldspaceOffset = new Vector3d(0, 0, 0);
        Quaternion worldspaceRotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);

        double current = Epoch.CurrentMilliseconds() / 1000.0;
        double useTime = current * timeFactor;
        PositionOrbitalParent(anchor, worldspaceOffset, worldspaceRotation, useTime, null);
    }
    public Star GetStar()
    {
        return _star;
    }
    public void SetOrbitalAnchor(Orbital newAnchor)
    {
        SetAnchor(newAnchor);
    }

    public void SetNewOrbitalAnchor(GameObject gameObject)
    {
        foreach (DictionaryEntry entry in orbitalMap)
        {
            if ((GameObject)(entry.Value) == gameObject)
            {
                if (entry.Key is OrbitalBody)
                {
                    OrbitalBody body = (OrbitalBody)entry.Key;

                    Debug.Log("Set new orbit anchor: " + body.name);

                    anchor.RemoveParent();
                    body.AddSatellite(anchor);
                    anchor.orbitRadius = body.radius * 1.4;

                    return;
                }
            }
        }
    }

    public void LoadStarSystem(Star starSystem)
    {
        if (!StarSystemIsLoaded())
        {
            _star = starSystem;

            LoadOrbital(_star);
            LoadDescendants(_star);
            //SetAnchor(_star);

            /*
            if (anchorObject != null)
            {
                ShipRenderer renderer = anchorObject.GetComponent<ShipRenderer>();
                if (renderer != null)
                {
                    renderer.SetLightSource(GetOrbitalObject(_star));
                }
            }
            */
        }

    }
    public void UnloadStarSystem()
    {
        if (StarSystemIsLoaded())
        {
            UnloadOrbital(_star);
            UnloadDescendants(_star);
            //SetAnchor(null);

            _star = null;
            //anchor = null;

            /*
            if (anchorObject != null)
            {
                ShipRenderer renderer = anchorObject.GetComponent<ShipRenderer>();
                if (renderer != null)
                {
                    renderer.SetLightSource(GetOrbitalObject(null));
                }
            }
            */
        }
    }
    public bool StarSystemIsLoaded()
    {
        return (_star != null);
    }

    public Orbital GetStrongestGravity(Vector3 fromPosition)
    {
        double strongestGravity = 0;
        Orbital rtval = null;
        foreach (DictionaryEntry entry in orbitalMap)
        {
            GameObject obj = (GameObject)entry.Value;
            if (entry.Key is OrbitalBody)
            {
                OrbitalBody body = (OrbitalBody)entry.Key;
                double dist = (obj.transform.position - fromPosition).magnitude * Numbers.UnitsToMeters;
                double gravity = body.CalculateGravityFromDistance(dist);
                if (gravity > strongestGravity)
                {
                    strongestGravity = gravity;
                    rtval = body;
                }
            }
        }
        return rtval;
    }

    void LoadOrbital(Orbital orbital)
    {
        if (!OrbitalIsLoaded(orbital))
        {
            GameObject go = null;
            if (orbital is Planet)
            {
                Planet planet = (Planet)orbital;
                go = Instantiate(planetPrefab);
                go.GetComponent<PlanetMono>().DisplayPlanet(planet);
                go.GetComponent<PlanetMono>().SetLightSource(GetOrbitalObject(_star));
            }
            else if (orbital is Star)
            {
                Star star = (Star)orbital;
                go = Instantiate(starPrefab);
                go.GetComponent<StarMono>().DisplayStar(star);
            }
            else if (orbital is Station)
            {
                go = Instantiate(stationPrefab);
            }
            else
            {
                go = Instantiate(nonePrefab);
            }

            if (go != null)
            {
                orbitalMap.Add(orbital, go);
                go.transform.SetParent(transform);
                go.name = orbital.name;

                if (orbital is OrbitalBody)
                {
                    OrbitalBody body = (OrbitalBody)orbital;

                    double radiusUnits = (body.radius / Numbers.UnitsToMeters);
                    Observable obs = go.GetComponent<Observable>();
                    if (obs != null)
                    {
                        obs.minZoom = (float)(body.radius * 1.2);
                        obs.maxZoom = (float)(body.radius * 4);
                        obs.defaultZoom = (float)(body.radius * 1.5);
                    }
                }

                InteractableManager.Instance.Register(go);
                Interactable interactable = InteractableManager.Instance.GetInteractable(go);
                if (interactable)
                {
                    interactable.SetNameLabel(orbital.name);

                    if (orbital is OrbitalBody)
                    {
                        double dist = ((OrbitalBody)orbital).radius;
                        interactable.SetOffsetDistance(dist);
                    }
                }
                else
                {
                    Debug.Log("No interactable for: " + orbital.name);
                }
            }
        }
    }
    void UnloadOrbital(Orbital orbital)
    {
        if (OrbitalIsLoaded(orbital))
        {
            GameObject go = (GameObject)orbitalMap[orbital];
            orbitalMap.Remove(orbital);
            InteractableManager.Instance.Unregister(go);
            Destroy(go);
        }
    }
    bool OrbitalIsLoaded(Orbital orbital)
    {
        return orbitalMap.ContainsKey(orbital);
    }
    GameObject GetOrbitalObject(Orbital orbital)
    {
        if (OrbitalIsLoaded(orbital))
        {
            return (GameObject)orbitalMap[orbital];
        }
        return null;
    }

    void SetAnchor(Orbital newAnchor)
    {
        anchor = newAnchor;
    }

    void UnloadChildren(OrbitalBody body)
    {
        foreach (Orbital satellite in body.satellites)
        {
            UnloadOrbital(satellite);
        }
    }
    void LoadChildren(OrbitalBody body)
    {
        foreach (Orbital satellite in body.satellites)
        {
            LoadOrbital(satellite);
        }
    }
    void UnloadDescendants(OrbitalBody body)
    {
        foreach (Orbital satellite in body.satellites)
        {
            UnloadOrbital(satellite);
            if (satellite is OrbitalBody)
            {
                UnloadDescendants((OrbitalBody)satellite);
            }
        }
    }
    void LoadDescendants(OrbitalBody body)
    {
        foreach (Orbital satellite in body.satellites)
        {
            LoadOrbital(satellite);
            if (satellite is OrbitalBody)
            {
                LoadDescendants((OrbitalBody)satellite);
            }
        }
    }




    void PositionOrbitalSelf(Orbital orbital, Vector3d worldspaceOffset, Quaternion worldspaceRotation, double currentTime)
    {
        if (OrbitalIsLoaded(orbital))
        {
            GameObject obj = (GameObject)orbitalMap[orbital];
            obj.transform.localPosition = (worldspaceOffset.ToUnity() / (float)Numbers.UnitsToMeters);
            obj.transform.rotation = worldspaceRotation * orbital.GetRotationalPeriodRotation(currentTime);
        }
    }
    void PositionOrbitalParent(Orbital orbital, Vector3d worldspaceOffset, Quaternion worldspaceRotation, double currentTime, Orbital fromChild)
    {
        PositionOrbitalChild(orbital, worldspaceOffset, worldspaceRotation, currentTime, fromChild);

        if (orbital.parent != null)
        {
            Quaternion orbitPlaneRotation = Quaternion.Inverse(orbital.GetAxialTiltRotation() * orbital.rotationOffset);
            Quaternion orbitRotation = worldspaceRotation * orbitPlaneRotation * orbital.GetOrbitPeriodRotation(currentTime);

            Vector3d orbitOffset = (orbitRotation * new Vector3(0, 0, (float)orbital.orbitRadius)).ToUnityd();

            Vector3d parentOffset = worldspaceOffset - orbitOffset;
            Quaternion parentRotation = worldspaceRotation * orbitPlaneRotation * Quaternion.Inverse(orbital.GetLongANRotation() * orbital.GetOrbitInclinationRotation());

            PositionOrbitalParent(orbital.parent, parentOffset, parentRotation, currentTime, orbital);
}
    }
    void PositionOrbitalChild(Orbital orbital, Vector3d worldspaceOffset, Quaternion worldspaceRotation, double currentTime, Orbital ignoreChild)
    {
        PositionOrbitalSelf(orbital, worldspaceOffset, worldspaceRotation, currentTime);

        if (orbital is OrbitalBody)
        {
            OrbitalBody body = (OrbitalBody)orbital;
            foreach (Orbital child in body.satellites)
            {
                if (child != ignoreChild)
                {
                    Quaternion orbitPlaneRotation = child.GetLongANRotation() * child.GetOrbitInclinationRotation();
                    Quaternion orbitRotation = worldspaceRotation * orbitPlaneRotation * child.GetOrbitPeriodRotation(currentTime);
                    Quaternion tiltRotation = child.GetAxialTiltRotation();

                    Vector3d orbitOffset = (orbitRotation * new Vector3(0, 0, (float)child.orbitRadius)).ToUnityd();

                    Vector3d childOffset = worldspaceOffset + orbitOffset;
                    Quaternion childRotation = worldspaceRotation * orbitPlaneRotation * tiltRotation;

                    PositionOrbitalChild(child, childOffset, childRotation, currentTime, null);
                }

            }
        }
    }
}
