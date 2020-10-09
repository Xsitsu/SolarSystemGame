using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemGeneratorSol : ISystemGenerator
{
    Color MakeColor(int r, int g, int b)
    {
        return new Color(r / 255f, g / 255f, b / 255f, 1);
    }
    Planet MakePlanet(double mass, double radiusKM, Color color, double orbitRadiusAU)
    {
        Planet planet = new Planet();
        planet.mass = mass * Numbers.EarthMassToKGs;
        planet.radius = radiusKM * 1000;
        planet.color = color;
        planet.orbitRadius = orbitRadiusAU * Numbers.AUToKM * 1000;
        return planet;
    }
    Planet MakeMoon(double mass, double radiusKM, Color color, double orbitRadiusKM)
    {
        Planet planet = new Planet();
        planet.mass = mass;
        planet.radius = radiusKM * 1000;
        planet.color = color;
        planet.orbitRadius = orbitRadiusKM * 1000;
        return planet;
    }
    public Star Generate()
    {
        Star sun = new Star();
        sun.mass = Numbers.SunMassToKGs;
        sun.radius = 696340 * 1000;
        sun.color = MakeColor(252, 212, 64);

        sun.AddSatellite(MakePlanet(0.055, 2439.7, MakeColor(128, 106, 75), 0.39)); // Mercury
        sun.AddSatellite(MakePlanet(0.815, 6051.8, MakeColor(207, 184, 54), 0.723)); // Venus
        sun.AddSatellite(MakePlanet(1.000, 6378.1, MakeColor(49, 108, 196), 1.000)); // Earth
        sun.AddSatellite(MakePlanet(0.107, 3396.2, MakeColor(189, 66, 28), 1.524)); // Mars
        sun.AddSatellite(MakePlanet(317.8, 71492, MakeColor(207, 128, 89), 5.203)); // Jupiter
        sun.AddSatellite(MakePlanet(95.16, 60268, MakeColor(247, 200, 57), 9.539)); // Saturn
        sun.AddSatellite(MakePlanet(14.54, 25559, MakeColor(51, 191, 222), 19.18)); // Uranus
        sun.AddSatellite(MakePlanet(17.15, 24764, MakeColor(6, 57, 199), 30.06)); // Neptune
        sun.AddSatellite(MakePlanet(0.0022, 1195, MakeColor(186, 185, 182), 39.53)); // Pluto

        sun.satellites[2].AddSatellite(MakeMoon(7.342 * System.Math.Pow(10, 22), 1737.4, MakeColor(220, 220, 220), 384400)); // Earth's Moon

        return sun;
    }
}
