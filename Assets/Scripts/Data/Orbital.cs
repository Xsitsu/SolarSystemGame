using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbital
{
    public double orbitRadius; // m
    public List<Orbital> satellites = new List<Orbital>();

    public double CalculateOrbitalPeriod(double massCentralKG)
    {
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
}
