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

    public Orbital anchor;
    public Orbital currentAnchor;

    [Range(0.000001f, 1000)]
    public double timeFactor = 1.0;
    public int dayOffset = 0;
    public GameObject starPrefab;
    public GameObject planetPrefab;

    List<SolarSystemObject> bodies = new List<SolarSystemObject>();
    int last = -1;

    void AddBodyToList(Orbital body, Transform parent)
    {
        SolarSystemObject obj = new SolarSystemObject();
        obj.Body = body;
        obj.Object = null;

        if (body is Planet)
        {
            GameObject go = Instantiate(planetPrefab);
            obj.Object = go;
            go.transform.SetParent(parent);
            go.GetComponent<PlanetMono>().DisplayPlanet((Planet)body);
        }
        if (body is Star)
        {
            GameObject go = Instantiate(starPrefab);
            obj.Object = go;
            go.transform.SetParent(parent);
            go.GetComponent<StarMono>().DisplayStar((Star)body);
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
        currentAnchor = anchor;

        AddBodyToList(system, transform);
    }
    void UnloadSolarSystem()
    {
        foreach (SolarSystemObject body in bodies)
        {
            Destroy(body.Object);
        }

        bodies.Clear();
    }
    void Start()
    {
        LoadSolarSystem(new SystemGeneratorSol().Generate());
        currentAnchor = anchor.satellites[2].satellites[0];
    }
    void Update()
    {
        int current = Epoch.Current();
        current += dayOffset * Numbers.DayToSeconds;
        if (current != last)
        {
            last = current;

            foreach (SolarSystemObject body in bodies)
            {
                if (body.Body != anchor)
                {
                    float radM = (float)body.Body.orbitRadius;
                    float radAU = (radM / 1000) / Numbers.AUToKM;
                    int periodSeconds = (int)(Numbers.YearToSeconds * Mathf.Sqrt(radAU * radAU * radAU));
                    int currentPeriod = (int)(current * timeFactor) % periodSeconds;
                    float percent = (float)currentPeriod / (float)periodSeconds;

                    float x = percent * Mathf.PI * 2;
                    float z = percent * Mathf.PI * 2;
                    Vector3 dir =  new Vector3(Mathf.Cos(x), 0, Mathf.Sin(z));

                    if (body.Object)
                    {
                        body.Object.transform.localPosition = dir * (float)(radM / Numbers.UnitsToMeters);
                    }
                }
            }

            if (currentAnchor != null)
            {
                foreach (SolarSystemObject body in bodies)
                {
                    if (body.Body == currentAnchor)
                    {
                        transform.localPosition -= body.Object.transform.position;
                    }
                }
            }
        }
    }
}
