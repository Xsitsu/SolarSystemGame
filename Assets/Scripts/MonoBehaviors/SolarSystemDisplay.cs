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
    void Awake()
	{
		_instance = this;
	}

    void OnDestroy()
    {
        _instance = null;
    }

    void SetupOrbitalBody(GameObject go, OrbitalBody orbitalBody)
    {
        double radiusUnits = (orbitalBody.radius / Numbers.UnitsToMeters);
        Observable obs = go.GetComponent<Observable>();
        obs.minZoom = (float)(orbitalBody.radius * 1.2);
        obs.maxZoom = (float)(orbitalBody.radius * 4);
        obs.zoomSpeed = (float)(orbitalBody.radius * 0.5);
        obs.defaultZoom = (float)(orbitalBody.radius * 1.5);

        InteractableManager.Instance.Register(go);
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

        foreach (Orbital orbital in body.satellites)
        {
            Transform setParent = null;
            if (obj.Object != null)
            {
                setParent = obj.Object.GetComponent<OrbitalBodyMono>().satellites.transform;
            }

            AddBodyToList(orbital, setParent);
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

        foreach (Orbital satellite in orbital.satellites)
        {
            UpdateOrbital(currentTime, satellite);
        }
    }
    void Start()
    {
        LoadSolarSystem(new SystemGeneratorSol().Generate());
    }
    void Update()
    {
        double current = Epoch.CurrentMilliseconds() / 1000.0;
        current += dayOffset * Numbers.DayToSeconds;
        if (current != last)
        {
            last = current;

            UpdateOrbital(current * timeFactor, anchor);
        }

        if (anchorObject != null)
        {
            transform.localPosition -= anchorObject.transform.position;
        }
    }
}
