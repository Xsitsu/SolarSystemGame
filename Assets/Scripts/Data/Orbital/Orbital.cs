using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OrbitalMechanics;

public class Orbital : Entity
{
    public OrbitalMechanics.Orbit orbit_m = new OrbitalMechanics.Orbit();

    public void SetOrbitData(double eccentricity, double semiMajorAxis_m, double inclination_deg,
        double longitudeOfAN_deg, double argumentOfPeriapsis_deg, double periapsisEpoch_sec)
    {
        this.orbit_m.SetOrbitData(semiMajorAxis_m, eccentricity, inclination_deg, longitudeOfAN_deg, argumentOfPeriapsis_deg, periapsisEpoch_sec);
    }

    public override Vector3d CalculatePosition(double atTime)
    {
        if (parent != null)
        {
            Quaternion orbitPlaneRotation = GetLongANRotation() * GetInclinationRotation();

            OrbitalMechanics.Offset offset = UniverseHandler.Instance.GetSolver().CalculateOffset_m(parent.mass_kg, orbit_m, atTime);
            Vector3 orbitOffset = new Vector3((float)offset.X, 0, (float)offset.Y);

            Vector3d orbitPosition = (orbitPlaneRotation * orbitOffset).ToUnityd();
            return orbitPosition;
        }
        return new Vector3d(0, 0, 1);
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

    // TODO: Refactor and rename method since it's not "calculating" anything anymore
    public double CalculateOrbitalPeriod_sec()
    {
        if (parent != null)
        {
            return UniverseHandler.Instance.GetSolver().CalculateOrbitalPeriod_sec(parent.mass_kg, orbit_m);
        }
        return 0;
    }

    public Quaternion GetLongANRotation()
    {
        return Quaternion.Euler(0, -(float)(orbit_m.LongitudeOfAN_deg), 0);
    }
    public Quaternion GetInclinationRotation()
    {
        return Quaternion.Euler(0, 0, (float)(orbit_m.Inclination_deg));
    }
    // public Quaternion GetOrbitPeriodRotation(double atTime)
    // {
    //     if (parent != null && orbit_m.SemiMajorAxis_m > 0)
    //     {
    //         double period_sec = CalculateOrbitalPeriod_sec();

    //         double elapsedSinceEpoch = atTime - periapsisEpoch_sec;

    //         double currentPeriod_sec = elapsedSinceEpoch % period_sec;
    //         double percent = currentPeriod_sec / period_sec;

    //         percent %= 1;

    //         return Quaternion.Euler(0, -(float)((percent * 360) + argumentOfPeriapsis_deg), 0);
    //     }
    //     return Quaternion.LookRotation(Vector3.forward, Vector3.up);
    // }
    // public double GetOrbitPeriodDistance_m(double atTime)
    // {
    //     // TODO: Eventually implement eccentric orbits
    //     // for now everything is projected onto a sphere

    //     return semiMajorAxis_m;
    // }
}
