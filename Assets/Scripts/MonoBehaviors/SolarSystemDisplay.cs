using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystemDisplay : MonoBehaviour
{
    public class SolarSystemObject
    {
        public Orbital Body;
        public GameObject Object;
    }

    [Range(0.000001f, 1000000)]
    public double timeFactor = 1.0;
    public int dayOffset = 0;
    public GameObject starPrefab;
    public GameObject planetPrefab;

    public Orbital anchor;
    public GameObject anchorObject;

    List<SolarSystemObject> bodies = new List<SolarSystemObject>();
    double last = -1;

    static private SolarSystemDisplay _instance;
	static public SolarSystemDisplay Instance { get { return _instance; } }
    void SetupOrbitalBody(GameObject go, OrbitalBody orbitalBody)
    {
        double radiusUnits = (orbitalBody.radius / Numbers.UnitsToMeters);
        Observable obs = go.GetComponent<Observable>();
        obs.minZoom = (float)(orbitalBody.radius * 1.2);
        obs.maxZoom = (float)(orbitalBody.radius * 4);
        obs.zoomSpeed = (float)(orbitalBody.radius * 0.5);
        obs.defaultZoom = (float)(orbitalBody.radius * 1.5);

        InteractableManager.Instance.Register(go);
        Interactable interactable = InteractableManager.Instance.GetInteractable(go);
        if (interactable)
        {
            interactable.SetNameLabel(orbitalBody.name);
        }
        else
        {
            Debug.Log("No interactable for: " + orbitalBody.name);
        }
    }

    void AddBodyToList(Orbital body, Transform parent)
    {
        SolarSystemObject obj = new SolarSystemObject();
        obj.Body = body;
        obj.Object = null;

        if (body is Planet)
        {
            Planet planet = (Planet)body;

            GameObject go = Instantiate(planetPrefab);
            obj.Object = go;
            go.transform.SetParent(parent);
            go.GetComponent<PlanetMono>().DisplayPlanet(planet);

            SetupOrbitalBody(go, planet);
        }
        if (body is Star)
        {
            Star star = (Star)body;

            GameObject go = Instantiate(starPrefab);
            obj.Object = go;
            go.transform.SetParent(parent);
            go.GetComponent<StarMono>().DisplayStar(star);

            SetupOrbitalBody(go, star);
        }

        bodies.Add(obj);

        if (body is OrbitalBody)
        {
            foreach (Orbital orbital in ((OrbitalBody)body).satellites)
            {
                Transform setParent = null;
                if (obj.Object != null)
                {
                    setParent = obj.Object.GetComponent<OrbitalBodyMono>().satellites.transform;
                }

                AddBodyToList(orbital, setParent);
            }
        }
    }
    void LoadSolarSystem(Orbital system)
    {
        anchor = system;

        AddBodyToList(system, transform);
    }
    void UnloadSolarSystem()
    {
        foreach (SolarSystemObject body in bodies)
        {
            InteractableManager.Instance.Unregister(body.Object);
            Destroy(body.Object);
        }

        bodies.Clear();
    }
    SolarSystemObject GetObjectFromOrbital(Orbital orbital)
    {
        foreach (SolarSystemObject sso in bodies)
        {
            if (sso.Body == orbital)
            {
                return sso;
            }
        }
        return null;
    }
    void UpdateOrbital(double currentTime, Orbital orbital)
    {
        SolarSystemObject sso = GetObjectFromOrbital(orbital);
        if (sso != null)
        {
            if (sso.Object)
            {
                double radM = orbital.orbitRadius;
                Vector3 dir = orbital.CalculateRelativeDirection(currentTime).ToUnity();
                sso.Object.transform.localPosition = dir.normalized * (float)(radM / Numbers.UnitsToMeters);
            }
        }

        if (orbital is OrbitalBody)
        {
            foreach (Orbital satellite in ((OrbitalBody)orbital).satellites)
            {
                UpdateOrbital(currentTime, satellite);
            }
        }
    }
    public SolarSystemObject GetStrongestGravity(Vector3 fromPosition)
    {
        double strongestGravity = 0;
        SolarSystemObject rtval = null;
        foreach (SolarSystemObject sso in bodies)
        {
            if (sso.Object != null && sso.Body != null)
            {
                if (sso.Body is OrbitalBody)
                {
                    double dist = (sso.Object.transform.position - fromPosition).magnitude * Numbers.UnitsToMeters;
                    double gravity = ((OrbitalBody)sso.Body).CalculateGravityFromDistance(dist);
                    if (gravity > strongestGravity)
                    {
                        strongestGravity = gravity;
                        rtval = sso;
                    }
                }
                
            }
        }
        return rtval;
    }
    public SolarSystemObject GetClosestToSurface(Vector3 fromPosition)
    {
        double closestDist = 0;
        SolarSystemObject rtval = null;
        foreach (SolarSystemObject sso in bodies)
        {
            if (sso.Object != null && sso.Body != null)
            {
                if (sso.Body is OrbitalBody)
                {
                    double dist = (sso.Object.transform.position - fromPosition).magnitude * Numbers.UnitsToMeters;
                    dist -= ((OrbitalBody)sso.Body).radius;
                    if (dist < 0)
                    {
                        dist = 0;
                        return sso;
                    }
                    if (closestDist > dist)
                    {
                        closestDist = dist;
                        rtval = sso;
                    }
                }
                
            }
        }
        return rtval;
    }
    void PositionOrbitalSelf(Orbital orbital, Vector3d offset)
    {
        SolarSystemObject sso = GetObjectFromOrbital(orbital);
        if (sso != null && sso.Object != null)
        {
            sso.Object.transform.localPosition = (offset.ToUnity() / (float)Numbers.UnitsToMeters);
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
        //LoadSolarSystem(new SystemGeneratorSol().Generate());
        LoadSolarSystem(new GalaxyGeneratorTest().Generate(42));
        foreach (SolarSystemObject sso in bodies)
        {
            if (sso.Object != null)
            {
                sso.Object.transform.SetParent(transform);
            }
        }
    }
    void Update()
    {
        if (anchorObject != null)
        {
            transform.localPosition -= anchorObject.transform.position;

            SolarSystemDisplay.SolarSystemObject sso = GetStrongestGravity(anchorObject.transform.position);
            if (sso != null && anchor != sso.Body)
            {
                //Debug.Log("Changing anchor to: " + sso.Body.name);
                
                Vector3 diff = anchorObject.transform.position - sso.Object.transform.position;
                anchorObject.transform.localPosition = diff;
                transform.localPosition = -diff;
                anchor = sso.Body;
            }
            
        }

        double current = Epoch.CurrentMilliseconds() / 1000.0;
        current += dayOffset * Numbers.DayToSeconds;
        if (current != last)
        {
            last = current;
            double useTime = current * timeFactor;
            PositionOrbitalParent(anchor, new Vector3d(0, 0, 0), useTime, null);
        }
    }
}
