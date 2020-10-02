using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemGeneratorSol : ISystemGenerator
{
    Color MakeColor(int r, int g, int b)
    {
        return new Color(r / 255f, g / 255f, b / 255f, 1);
    }
    Planet MakePlanet(double mass, double radius, Color color, double orbitRadius)
    {
        Planet planet = new Planet();
        planet.mass = mass;
        planet.radius = radius;
        planet.color = color;
        planet.orbitRadius = orbitRadius;
        return planet;
    }
    public Star Generate()
    {
        Star sun = new Star();
        sun.mass = 1.989 * (10^30);
        sun.radius = 696340;
        sun.color = MakeColor(252, 212, 64);

        sun.satellites.Add(MakePlanet(0.055, 2439.7, MakeColor(128, 106, 75), 0.39)); // Mercury
        sun.satellites.Add(MakePlanet(0.815, 6051.8, MakeColor(207, 184, 54), 0.723)); // Venus
        sun.satellites.Add(MakePlanet(1, 6378.1, MakeColor(49, 108, 196), 1)); // Earth
        sun.satellites.Add(MakePlanet(0.107, 3396.2, MakeColor(189, 66, 28), 1.524)); // Mars
        sun.satellites.Add(MakePlanet(317.8, 71492, MakeColor(207, 128, 89), 5.203)); // Jupiter
        sun.satellites.Add(MakePlanet(95.16, 60268, MakeColor(247, 200, 57), 9.539)); // Saturn
        sun.satellites.Add(MakePlanet(14.54, 25559, MakeColor(51, 191, 222), 19.18)); // Uranus
        sun.satellites.Add(MakePlanet(17.15, 24764, MakeColor(6, 57, 199), 30.06)); // Neptune
        sun.satellites.Add(MakePlanet(0.0022, 1195, MakeColor(186, 185, 182), 39.53)); // Pluto

        return sun;
    }
}
