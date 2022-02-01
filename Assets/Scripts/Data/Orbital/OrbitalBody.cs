using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalBody : Orbital
{
    public double axialTilt_deg;
    public double rotationalPeriod_sec; // sidereal

    public override Quaternion CalculateRotation(double atTime)
    {
        Quaternion orbitPlaneRotation = base.CalculateRotation(atTime);
        return orbitPlaneRotation * GetAxialTiltRotation();
    }
    public override Quaternion CalculateLocalRotation(double atTime)
    {
        return GetRotationalPeriodRotation(atTime);
    }
    public Quaternion GetAxialTiltRotation()
    {
        return Quaternion.Euler(0, 0, (float)axialTilt_deg);
    }
    public Quaternion GetRotationalPeriodRotation(double atTime)
    {
        double period_sec = rotationalPeriod_sec;
        double currentPeriod_sec = atTime % period_sec;
        double percent = currentPeriod_sec / period_sec;
        percent %= 1;

        return Quaternion.Euler(0, (float)(percent * 360), 0);
    }
}
