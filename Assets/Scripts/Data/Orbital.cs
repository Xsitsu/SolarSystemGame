using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbital
{
    public string name = "";
    public double orbitRadius; // m
    public Vector3d offset; // m
    public double orbitPercentOffset;
    public bool anchored = false;
    public OrbitalBody parent { get; private set; }

    public bool HasAncestor(OrbitalBody ancestor)
    {
        OrbitalBody check = parent;
        while (check != null)
        {
            if (check == ancestor)
            {
                return true;
            }
            else
            {
                check = check.parent;
            }
        }
        return false;
    }
    public OrbitalBody FindCommonAncestor(Orbital other)
    {
        OrbitalBody check = parent;
        while (check != null && !other.HasAncestor(check))
        {
            check = check.parent;
        }
        return check;
    }
    public void AddTo(OrbitalBody parent)
    {
        this.parent = parent;
    }
    public void RemoveFrom(OrbitalBody parent)
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
            percent += orbitPercentOffset;
            percent %= 1;

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
