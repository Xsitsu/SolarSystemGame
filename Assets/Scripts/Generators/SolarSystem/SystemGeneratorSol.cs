using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemGeneratorSol : ISystemGenerator
{
    Color MakeColor(int r, int g, int b)
    {
        return new Color(r / 255f, g / 255f, b / 255f, 1);
    }
    Planet MakePlanet(string name, double mass, double radiusKM, Color color, double orbitRadiusAU)
    {
        Planet planet = new Planet();
        planet.name = name;
        planet.mass = mass * Numbers.EarthMassToKGs;
        planet.radius = radiusKM * 1000;
        planet.color = color;
        planet.orbitRadius = orbitRadiusAU * Numbers.AUToKM * 1000;
        return planet;
    }
    Planet MakeMoon(string name, double mass, double radiusKM, Color color, double orbitRadiusKM)
    {
        Planet planet = new Planet();
        planet.name = name;
        planet.mass = mass;
        planet.radius = radiusKM * 1000;
        planet.color = color;
        planet.orbitRadius = orbitRadiusKM * 1000;
        return planet;
    }
    public Star Generate()
    {
        Star sun = new Star();
        sun.name = "Sol";
        sun.mass = Numbers.SunMassToKGs;
        sun.radius = 696340 * 1000;
        sun.color = MakeColor(252, 212, 64);

        // Mercury
        Planet mercury = MakePlanet("Mercury", 0.055, 2439.7, MakeColor(128, 106, 75), 0.39);

        // Venus
        Planet venus = MakePlanet("Venus", 0.815, 6051.8, MakeColor(207, 184, 54), 0.723);

        // Earth
        Planet earth = MakePlanet("Earth", 1.000, 6378.1, MakeColor(49, 108, 196), 1.000);
        Planet earthMoon = MakeMoon("Luna", 7.342 * System.Math.Pow(10, 22), 1737.4, MakeColor(220, 220, 220), 384400);

        Planet earthMoonMoon = MakeMoon("Luna Minor", 1 * System.Math.Pow(10, 20), 10, MakeColor(255, 0, 255), (earthMoon.radius / 1000) + 10000);

        earth.AddSatellite(earthMoon);
        earthMoon.AddSatellite(earthMoonMoon);

        // Mars
        Planet mars = MakePlanet("Mars", 0.107, 3396.2, MakeColor(189, 66, 28), 1.524);

        Planet phobos = MakeMoon("Phobos", 10.6 * System.Math.Pow(10, 15), 11, MakeColor(220, 220, 220), 9376);
        Planet deimos = MakeMoon("Deimos", 1.4762 * System.Math.Pow(10, 15), 6.2, MakeColor(208, 208, 192), 23463.2);

        mars.AddSatellite(phobos);
        mars.AddSatellite(deimos);

        // Jupiter
        Planet jupiter = MakePlanet("Jupiter", 317.8, 71492, MakeColor(207, 128, 89), 5.203);

        // Saturn
        Planet saturn = MakePlanet("Saturn", 95.16, 60268, MakeColor(247, 200, 57), 9.539);

        // Uranus
        Planet uranus = MakePlanet("Uranus", 14.54, 25559, MakeColor(51, 191, 222), 19.18);

        // Neptune
        Planet neptune = MakePlanet("Neptune", 17.15, 24764, MakeColor(6, 57, 199), 30.06);
        
        // Pluto
        Planet pluto = MakePlanet("Pluto", 0.0022, 1195, MakeColor(186, 185, 182), 39.53);

        sun.AddSatellite(mercury);
        sun.AddSatellite(venus);
        sun.AddSatellite(earth);
        sun.AddSatellite(mars);
        sun.AddSatellite(jupiter);
        sun.AddSatellite(saturn);
        sun.AddSatellite(uranus);
        sun.AddSatellite(neptune);
        sun.AddSatellite(pluto);

        return sun;
    }
}
