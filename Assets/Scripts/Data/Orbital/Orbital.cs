using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbital : Entity
{
    public double orbitRadius; // meters
    public double orbitInclination; // degrees (relative to parent equator)
    public double longitudeOfAN; // degrees
    public double axialTilt; // degrees (relative to orbit)
    public double rotationalPeriod; // seconds (sidereal)

    public Vector3d offset; // meters
    public double orbitPercentOffset;
    public Quaternion rotationOffset = Quaternion.LookRotation(Vector3.forward, Vector3.up);

    public bool anchored = false;

    public override Vector3d CalculatePosition(double atTime)
    {
        Quaternion orbitPlaneRotation = GetLongANRotation() * GetOrbitInclinationRotation();
        Quaternion orbitRotation = orbitPlaneRotation * GetOrbitPeriodRotation(atTime);

        Vector3d orbitPosition = (orbitRotation * new Vector3(0, 0, (float)orbitRadius)).ToUnityd();
        return orbitPosition;
    }
    public override Quaternion CalculateRotation(double atTime)
    {
        Quaternion orbitPlaneRotation = GetLongANRotation() * GetOrbitInclinationRotation();
        Quaternion tiltRotation = GetAxialTiltRotation();

        Quaternion childRotation = orbitPlaneRotation * tiltRotation;
        return childRotation;
    }

    public double CalculateOrbitalPeriod()
    {
        if (parent != null && parent is OrbitalBody)
        {
            OrbitalBody par = (OrbitalBody)parent;
            double massCentralKG = par.mass;

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

    public Quaternion GetLongANRotation()
    {
        return Quaternion.Euler(0, (float)longitudeOfAN, 0);
    }
    public Quaternion GetOrbitInclinationRotation()
    {
        return Quaternion.Euler(0, 0, (float)orbitInclination);
    }
    public Quaternion GetAxialTiltRotation()
    {
        return Quaternion.Euler(0, 0, (float)axialTilt);
    }
    public Quaternion GetOrbitPeriodRotation(double atTime)
    {
        if (parent != null)
        {
            double percent = 0;
            if (!anchored)
            {
                double periodSeconds = CalculateOrbitalPeriod();
                double currentPeriod = atTime % periodSeconds;
                percent = currentPeriod / periodSeconds;
            }
            percent += orbitPercentOffset;
            percent %= 1;

            return Quaternion.Euler(0, (float)(percent * 360), 0);
        }
        return new Quaternion(0, 0, 0, 0);
    }
    public Quaternion GetRotationalPeriodRotation(double atTime)
    {
        double percent = 0;
        if (!anchored)
        {
            double periodSeconds = rotationalPeriod;
            double currentPeriod = atTime % periodSeconds;
            percent = currentPeriod / periodSeconds;
        }
        percent %= 1;

        return Quaternion.Euler(0, (float)(percent * 360), 0);
    }
    public System.Numerics.Vector3 CalculateRelativeDirection(double atTime)
    {
        if (parent != null)
        {
            double percent = 0;
            if (!anchored)
            {
                double periodSeconds = CalculateOrbitalPeriod();
                double currentPeriod = atTime % periodSeconds;
                percent = currentPeriod / periodSeconds;
            }
            percent += orbitPercentOffset;
            percent %= 1;
            /*
            double radM = orbitRadius;
            double radKM = (radM / 1000);
            double radAU = radKM / Numbers.AUToKM;
            double periodYears = periodSeconds / Numbers.YearToSeconds;
            */

            /*
            double x = percent * Numbers.PI2;
            double z = percent * Numbers.PI2;

            return new System.Numerics.Vector3((float)System.Math.Cos(x), 0, (float)System.Math.Sin(z));
            */

            Quaternion rotationLongAN = Quaternion.Euler(0, (float)longitudeOfAN, 0);
            Quaternion rotationOrbitInc = Quaternion.Euler(0, 0, (float)orbitInclination);
            Quaternion rotationOrbitPeriod = Quaternion.Euler(0, (float)(percent * 360), 0);


            Quaternion rotationTilt = Quaternion.Euler(0, 0, (float)axialTilt);

        }
        return System.Numerics.Vector3.Zero;
    }
}
