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
    public Vector3 position;

    [Range(0.000001f, 1000)]
    public double timeFactor = 1.0;
    public GameObject planetPrefab;
    public Shader planetShader;

    List<SolarSystemObject> bodies = new List<SolarSystemObject>();
    int last = -1;

    void AddBodyToList(Orbital body)
    {
        SolarSystemObject obj = new SolarSystemObject();
        obj.Body = body;
        obj.Object = null;

        if (body is Planet)
        {
            GameObject go = Instantiate(planetPrefab);
            Material mat = new Material(planetShader);
            mat.SetColor("_Color", ((Planet)body).color);
            go.GetComponent<MeshRenderer>().material = mat;
            obj.Object = go;

            float radiusKM = (float)((Planet)body).radius;
            go.transform.localScale *= (radiusKM * 1000) / Numbers.UnitsToMeters;
            go.transform.SetParent(transform);
        }
        if (body is Star)
        {
            GameObject go = Instantiate(planetPrefab);
            Material mat = new Material(planetShader);
            mat.SetColor("_Color", ((Star)body).color);
            go.GetComponent<MeshRenderer>().material = mat;
            obj.Object = go;

            go.transform.localScale *= (float)((Star)body).radius / Numbers.UnitsToMeters;
            go.transform.SetParent(transform);
        }

        bodies.Add(obj);

        foreach (Orbital orbital in body.satellites)
        {
            AddBodyToList(orbital);
        }
    }
    void LoadSolarSystem(Orbital system)
    {
        Vector3 dir = (new Vector3(1, -2, 0.7f)).normalized;
        float dist = 100;

        if (system is OrbitalBody)
        {
            dist = (float)((OrbitalBody)system).radius * 1.2f;
        }

        anchor = system;
        currentAnchor = anchor;
        position = dir * dist;

        AddBodyToList(system);
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
        currentAnchor = anchor.satellites[2];
    }
    void Update()
    {
        int current = Epoch.Current();
        if (current != last)
        {
            last = current;

            foreach (SolarSystemObject body in bodies)
            {
                if (body.Body != anchor)
                {
                    float rad = (float)body.Body.orbitRadius;
                    int periodSeconds = (int)(Numbers.YearToSeconds * Mathf.Sqrt(rad * rad * rad));
                    int currentPeriod = (int)(current * timeFactor) % periodSeconds;
                    float percent = (float)currentPeriod / (float)periodSeconds;

                    float x = percent * Mathf.PI * 2;
                    float z = percent * Mathf.PI * 2;
                    Vector3 dir =  new Vector3(Mathf.Cos(x), 0, Mathf.Sin(z));

                    if (body.Object)
                    {
                        float distM = rad * Numbers.AUToKM * 1000;
                        body.Object.transform.localPosition = dir * distM / Numbers.UnitsToMeters;
                    }
                }
            }

            if (currentAnchor != null)
            {
                foreach (SolarSystemObject body in bodies)
                {
                    if (body.Body == currentAnchor)
                    {
                        transform.localPosition = -body.Object.transform.localPosition;
                    }
                }
            }
        }
    }
}
