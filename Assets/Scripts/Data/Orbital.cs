using System.Collections;
using System.Collections.Generic;

public class Orbital
{
    public double mass; // kg
    public double orbitRadius; // m
    public Orbital parent { get; private set; }
    public List<Orbital> satellites = new List<Orbital>();

    public void AddSatellite(Orbital satellite)
    {
        if (!IsSatellite(satellite))
        {
            satellites.Add(satellite);
            satellite.AddTo(this);
        }
    }
    public void RemoveSatellite(Orbital satellite)
    {
        if (IsSatellite(satellite))
        {
            satellite.RemoveFrom(this);
            satellites.Remove(satellite);
        }
    }
    public bool IsSatellite(Orbital satellite)
    {
        foreach (Orbital s in satellites)
        {
            if (s == satellite)
            {
                return true;
            }
        }
        return false;
    }
    public void AddTo(Orbital parent)
    {
        this.parent = parent;
    }
    public void RemoveFrom(Orbital parent)
    {
        if (this.parent == parent)
        {
            this.parent = null;
        }
    }

    public double CalculateOrbitalPeriod()
    {
        if (parent != null)
        {
            double massCentralKG = parent.mass;

            double radM = orbitRadius;
            double rad3 = radM * radM * radM;
            double top = (4 * Numbers.PISquared * rad3);
            double bottom = (Numbers.G * massCentralKG);
            double period2 =  top / bottom;
            double period = System.Math.Sqrt(period2);

            //Debug.Log("Numbers.G: " + Numbers.G + " | massCentralKG: " + massCentralKG);
            //Debug.Log("radM: " + radM + " | rad3: " + rad3 + " | top: " + top + " | bottom: " + bottom + " | period2: " + period2 + " | period: " + period);

            return period;
        }
        return 0;
    }

    public System.Numerics.Vector3 CalculateRelativeDirection(double atTime)
    {
        if (parent != null)
        {
            double periodSeconds = CalculateOrbitalPeriod();
            double currentPeriod = atTime % periodSeconds;
            double percent = currentPeriod / periodSeconds;

            double radM = orbitRadius;
            double radKM = (radM / 1000);
            double radAU = radKM / Numbers.AUToKM;
            double periodYears = periodSeconds / Numbers.YearToSeconds;

            double x = percent * Numbers.PI2;
            double z = percent * Numbers.PI2;

            return new System.Numerics.Vector3((float)System.Math.Cos(x), 0, (float)System.Math.Sin(z));
        }
        return System.Numerics.Vector3.Zero;
    }
}
