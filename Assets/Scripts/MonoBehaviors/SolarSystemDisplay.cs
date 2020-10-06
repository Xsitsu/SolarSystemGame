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

    [Range(0.000001f, 1000000)]
    public double timeFactor = 1.0;
    public int dayOffset = 0;
    public GameObject starPrefab;
    public GameObject planetPrefab;

    List<SolarSystemObject> bodies = new List<SolarSystemObject>();
    double last = -1;

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
    void UpdateOrbital(double currentTime, Orbital orbital, OrbitalBody center)
    {
        if (center != null)
        {
            double periodSeconds = orbital.CalculateOrbitalPeriod(center.mass);
            double currentPeriod = (currentTime * timeFactor) % periodSeconds;
            double percent = currentPeriod / periodSeconds;

            double radM = orbital.orbitRadius;
            double radKM = (radM / 1000);
            double radAU = radKM / Numbers.AUToKM;
            double periodYears = periodSeconds / Numbers.YearToSeconds;

            //Debug.Log("periodSeconds:" + periodSeconds + " | currentPeriod " + currentPeriod + " | percent " + percent + " | periodYears " + periodYears);
            //Debug.Log("CenterMass: " + center.mass + "; Orbit (au): " + radAU + "; Period: " + (periodYears * 365.25) + ", Current: " + currentPeriod + "/" + periodSeconds + " : " + percent * 100 + "%");

            float x = (float)percent * Mathf.PI * 2;
            float z = (float)percent * Mathf.PI * 2;
            Vector3 dir = new Vector3(Mathf.Cos(x), 0, Mathf.Sin(z));

            SolarSystemObject sso = GetObjectFromOrbital(orbital);
            if (sso != null)
            {
                if (sso.Object)
                {
                    sso.Object.transform.localPosition = dir.normalized * (float)(radM / Numbers.UnitsToMeters);
                }

                if (currentAnchor == orbital)
                {
                    transform.localPosition -= sso.Object.transform.position;
                }
            }
        }

        if (orbital is OrbitalBody)
        {
            foreach (Orbital satellite in orbital.satellites)
            {
                UpdateOrbital(currentTime, satellite, (OrbitalBody)orbital);
            }
        }
    }
    void Start()
    {
        LoadSolarSystem(new SystemGeneratorSol().Generate());
        currentAnchor = anchor.satellites[2].satellites[0];
        //currentAnchor = anchor.satellites[0];
    }
    void Update()
    {
        double current = Epoch.CurrentMilliseconds() / 1000.0;
        current += dayOffset * Numbers.DayToSeconds;
        if (current != last)
        {
            last = current;

            UpdateOrbital(current, anchor, null);

            /*
            foreach (SolarSystemObject body in bodies)
            {
                if (body.Body != anchor)
                {
                    double radM = body.Body.orbitRadius;
                    double radKM = (radM / 1000);
                    double radAU = radKM / Numbers.AUToKM;
                    double periodYears = System.Math.Sqrt(radAU * radAU * radAU);//Mathf.Sqrt(radAU * radAU * radAU);
                    double periodSeconds = (Numbers.YearToSeconds * periodYears);
                    double currentPeriod = (current * timeFactor) % periodSeconds;
                    double percent = currentPeriod / periodSeconds;

                    Debug.Log("Orbit (au): " + radAU + "; Period: " + (periodYears * 365.25) + ", Current: " + currentPeriod + "/" + periodSeconds + " : " + percent * 100 + "%");

                    float x = (float)percent * Mathf.PI * 2;
                    float z = (float)percent * Mathf.PI * 2;
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
            */
        }
    }
}
