using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniverseDisplay : MonoBehaviour
{
    public GameObject nonePrefab;
    public GameObject starPrefab;
    public GameObject planetPrefab;

    public Universe universe;
    public Orbital anchor;
    public GameObject anchorObject;

    Hashtable orbitalMap = new Hashtable();

    static private UniverseDisplay _instance;
	static public UniverseDisplay Instance { get { return _instance; } }
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
        universe = new Universe();
        universe.galaxies.Add(new GalaxyGeneratorTest().Generate(42));
        SetAnchor(universe.galaxies[0]);
    }
    void Update()
    {
        if (anchorObject != null)
        {
            transform.localPosition -= anchorObject.transform.position;

            Orbital strongest = GetStrongestGravity(anchorObject.transform.position);
            if (strongest != null && strongest != anchor)
            {
                if (OrbitalIsLoaded(strongest))
                {
                    Debug.Log("Set new strongest: " + strongest.name);
                    GameObject obj = (GameObject)(orbitalMap[strongest]);

                    Vector3 diff = anchorObject.transform.position - obj.transform.position;
                    anchorObject.transform.localPosition = diff;
                    transform.localPosition = -diff;
                    SetAnchor(strongest);
                }
                else
                {
                    Debug.Log("Set new strongest [BAD]: " + strongest.name);
                }
            }
        }

        double current = Epoch.CurrentMilliseconds() / 1000.0;
        //current += dayOffset * Numbers.DayToSeconds;
        double useTime = current;// * timeFactor;
        PositionOrbitalParent(anchor, new Vector3d(0, 0, 0), useTime, null);
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
            }
            else if (orbital is Star)
            {
                Star star = (Star)orbital;
                go = Instantiate(starPrefab);
                go.GetComponent<StarMono>().DisplayStar(star);
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
                    obs.minZoom = (float)(body.radius * 1.2);
                    obs.maxZoom = (float)(body.radius * 4);
                    obs.zoomSpeed = (float)(body.radius * 0.5);
                    obs.defaultZoom = (float)(body.radius * 1.5);
                }

                InteractableManager.Instance.Register(go);
                Interactable interactable = InteractableManager.Instance.GetInteractable(go);
                if (interactable)
                {
                    interactable.SetNameLabel(orbital.name);
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
    
    public void SetAnchor(Orbital newAnchor)
    {
        if (anchor != null)
        {
            bool oldInNew = false;
            bool newInOld = false;
            if (newAnchor is OrbitalBody)
            {
                oldInNew = anchor.HasAncestor((OrbitalBody)newAnchor);
            }
            if (anchor is OrbitalBody)
            {
                newInOld = newAnchor.HasAncestor((OrbitalBody)anchor);
            }
            
            if (oldInNew)
            {
                UnloadOrbital(anchor);
                if (anchor is OrbitalBody)
                {
                    UnloadChildren((OrbitalBody)anchor);
                }
                
                OrbitalBody travel = anchor.parent;
                while (travel != null && travel.parent != newAnchor)
                {
                    UnloadOrbital(travel);
                    UnloadChildren(travel);
                    travel = travel.parent;
                }
            }
            else if (newInOld)
            {

            }
            else
            {
                OrbitalBody common = anchor.FindCommonAncestor(newAnchor);
                OrbitalBody travel = anchor.parent;
                while (travel != null && travel.parent != common)
                {
                    UnloadOrbital(travel);
                    UnloadChildren(travel);
                    travel = travel.parent;
                }
            }
        }
        anchor = newAnchor;
        LoadAnchor(anchor);
    }
    void UnloadAnchor(Orbital anch)
    {
        UnloadOrbital(anch);
        if (anch is OrbitalBody)
        {
            UnloadDescendants((OrbitalBody)anch);
        }
    }
    void LoadAnchor(Orbital anch)
    {
        LoadOrbital(anch);
        if (anch is OrbitalBody)
        {
            LoadChildren((OrbitalBody)anch);
        }

        OrbitalBody useParent = anch.parent;
        while (useParent != null)
        {
            if (!OrbitalIsLoaded(useParent))
            {
                LoadOrbital(useParent);
                LoadChildren(useParent);
            }
            useParent = useParent.parent;
        }
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

    void PositionOrbitalSelf(Orbital orbital, Vector3d offset)
    {
        if (OrbitalIsLoaded(orbital))
        {
            GameObject obj = (GameObject)orbitalMap[orbital];
            obj.transform.localPosition = (offset.ToUnity() / (float)Numbers.UnitsToMeters);
        }
    }
    void PositionOrbitalParent(Orbital orbital, Vector3d offset, double currentTime, Orbital fromChild)
    {
        PositionOrbitalSelf(orbital, offset);

        if (orbital is OrbitalBody)
        {
            OrbitalBody body = (OrbitalBody)orbital;
            foreach (Orbital child in body.satellites)
            {
                if (child != fromChild)
                {
                    double radM = child.orbitRadius;
                    Vector3d childOffset = child.offset;
                    if (radM > 0)
                    {
                        Vector3d dir = child.CalculateRelativeDirection(currentTime).ToUnityd();
                        childOffset += dir.normalized * radM;
                    }
                    PositionOrbitalChild(child, offset + childOffset, currentTime);
                }
            }
        }

        if (orbital.parent != null)
        {
            double radM = orbital.orbitRadius;
            Vector3d parentOffset = orbital.offset;
            if (radM > 0)
            {
                Vector3d dir = orbital.CalculateRelativeDirection(currentTime).ToUnityd();
                parentOffset += dir.normalized * radM;
            }
            PositionOrbitalParent(orbital.parent, offset - parentOffset, currentTime, orbital);
        }
    }
    void PositionOrbitalChild(Orbital orbital, Vector3d offset, double currentTime)
    {
        PositionOrbitalSelf(orbital, offset);

        if (orbital is OrbitalBody)
        {
            OrbitalBody body = (OrbitalBody)orbital;
            foreach (Orbital child in body.satellites)
            {
                double radM = child.orbitRadius;
                Vector3d childOffset = child.offset;
                if (radM > 0)
                {
                    Vector3d dir = child.CalculateRelativeDirection(currentTime).ToUnityd();
                    childOffset += dir.normalized * radM;
                }
                PositionOrbitalChild(child, offset + childOffset, currentTime);
            }
        }
    }
}
