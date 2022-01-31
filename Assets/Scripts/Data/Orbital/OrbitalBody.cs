using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalBody : Orbital
{
    public double mass; // kg
    public double radius; // m
    public List<Orbital> satellites = new List<Orbital>();

    public void AddSatellite(Orbital satellite)
    {
        if (!HasSatellite(satellite))
        {
            satellites.Add(satellite);
            satellite.AddTo(this);
        }
    }
    public void RemoveSatellite(Orbital satellite)
    {
        if (HasSatellite(satellite))
        {
            satellite.RemoveFrom(this);
            satellites.Remove(satellite);
        }
    }
    public bool HasSatellite(Orbital satellite)
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

    public double CalculateGravityFromDistance(double distance)
    {
        return Numbers.G * (mass / (distance * distance));
    }
}
