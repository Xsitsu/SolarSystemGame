using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbital : Entity
{
    // Keplerian Orbit Variables
    public double eccentricity;                                         // Unitless
    public double semiMajorAxis_m;                                      // Meters
    public double inclination_deg;                                      // Degrees
    public double longitudeOfAN_deg;                                    // Longitude of Ascending Node in degrees
    public double argumentOfPeriapsis_deg;                              // Degrees
    public double periapsisEpoch_sec;                                   // Seconds 


    public void SetOrbitData(double eccentricity, double semiMajorAxis_m, double inclination_deg,
        double longitudeOfAN_deg, double argumentOfPeriapsis_deg, double periapsisEpoch_sec)
    {
        this.eccentricity = eccentricity;
        this.semiMajorAxis_m = semiMajorAxis_m;
        this.inclination_deg = inclination_deg;
        this.longitudeOfAN_deg = longitudeOfAN_deg;
        this.argumentOfPeriapsis_deg = argumentOfPeriapsis_deg;
        this.periapsisEpoch_sec = periapsisEpoch_sec;
    }

    public override Vector3d CalculatePosition(double atTime)
    {
        Quaternion orbitPlaneRotation = GetLongANRotation() * GetInclinationRotation();
        Quaternion orbitRotation = orbitPlaneRotation * GetOrbitPeriodRotation(atTime);

        float orbitDistance_m = (float)GetOrbitPeriodDistance_m(atTime);

        Vector3d orbitPosition = (orbitRotation * new Vector3(0, 0, orbitDistance_m)).ToUnityd();
        return orbitPosition;
    }
    public override Quaternion CalculateRotation(double atTime)
    {
        Quaternion orbitPlaneRotation = GetLongANRotation() * GetInclinationRotation();
        return orbitPlaneRotation;
    }
    public override Quaternion CalculateLocalRotation(double atTime)
    {
        return Quaternion.LookRotation(Vector3.forward, Vector3.up);
    }

    public double CalculateOrbitalPeriod_sec()
    {
        if (parent != null)
        {
            double SMA3 = semiMajorAxis_m * semiMajorAxis_m * semiMajorAxis_m;

            double top = (4 * Numbers.PISquared * SMA3);
            double bottom = (Numbers.G * parent.mass_kg);

            double period = System.Math.Sqrt(top / bottom);

            return period;
        }
        return 0;
    }

    public Quaternion GetLongANRotation()
    {
        return Quaternion.Euler(0, -(float)longitudeOfAN_deg, 0);
    }
    public Quaternion GetInclinationRotation()
    {
        return Quaternion.Euler(0, 0, (float)inclination_deg);
    }
    public Quaternion GetOrbitPeriodRotation(double atTime)
    {
        if (parent != null && semiMajorAxis_m > 0)
        {
            double period_sec = CalculateOrbitalPeriod_sec();

            double elapsedSinceEpoch = atTime - periapsisEpoch_sec;

            double currentPeriod_sec = elapsedSinceEpoch % period_sec;
            double percent = currentPeriod_sec / period_sec;

            percent %= 1;

            return Quaternion.Euler(0, -(float)((percent * 360) + argumentOfPeriapsis_deg), 0);
        }
        return Quaternion.LookRotation(Vector3.forward, Vector3.up);
    }
    public double GetOrbitPeriodDistance_m(double atTime)
    {
        // TODO: Eventually implement eccentric orbits
        // for now everything is projected onto a sphere

        return semiMajorAxis_m;
    }
}
