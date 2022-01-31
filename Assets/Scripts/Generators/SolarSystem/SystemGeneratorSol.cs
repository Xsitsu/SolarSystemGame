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

    void SetOrbitData(Orbital orbital, double orbitInclinationDegree, double longitudeOfANDegree, double axialTiltDegree, double rotationalPeriodSeconds)
    {
        orbital.orbitInclination = orbitInclinationDegree;
        orbital.longitudeOfAN = longitudeOfANDegree;
        orbital.axialTilt = axialTiltDegree;
        orbital.rotationalPeriod = rotationalPeriodSeconds;
    }

    public Star Generate()
    {
        Star sun = new Star();
        sun.name = "Sol";
        sun.mass = Numbers.SolarMassToKG;
        sun.radius = 696340 * 1000;
        sun.color = MakeColor(252, 212, 64);
        SetOrbitData(sun, 0, 0, 0, 25.05 * Numbers.DayToSeconds);

        // Mercury
        Planet mercury = MakePlanet("Mercury", 0.055, 2439.7, MakeColor(128, 106, 75), 0.39);
        SetOrbitData(mercury, 3.38, 48.331, 2.04, 58.646 * Numbers.DayToSeconds);

        // Venus
        Planet venus = MakePlanet("Venus", 0.815, 6051.8, MakeColor(207, 184, 54), 0.723);
        SetOrbitData(venus, 3.86, 76.680, 177.36, 243.0226 * Numbers.DayToSeconds);

        // Earth
        Planet earth = MakePlanet("Earth", 1.000, 6378.1, MakeColor(49, 108, 196), 1.000);
        SetOrbitData(earth, 7.155, -11.260, 23.439, 23 * Numbers.HourToSecond + 56 * Numbers.MinuteToSecond + 4.100);

        Planet earthMoon = MakeMoon("Luna", 7.342 * System.Math.Pow(10, 22), 1737.4, MakeColor(220, 220, 220), 384400);
        SetOrbitData(earthMoon, 5.145, 0, 6.687, 27.321661 * Numbers.DayToSeconds);

        //Planet earthMoonMoon = MakeMoon("Luna Minor", 1 * System.Math.Pow(10, 20), 10, MakeColor(255, 0, 255), (earthMoon.radius / 1000) + 10000);

        earth.AddChild(earthMoon);
        //earthMoon.AddSatellite(earthMoonMoon);

        // Mars
        Planet mars = MakePlanet("Mars", 0.107, 3396.2, MakeColor(189, 66, 28), 1.524);
        SetOrbitData(mars, 5.65, 49.558, 25.19, 1.025957 * Numbers.DayToSeconds);

        //Planet phobos = MakeMoon("Phobos", 10.6 * System.Math.Pow(10, 15), 11, MakeColor(220, 220, 220), 9376);
        //Planet deimos = MakeMoon("Deimos", 1.4762 * System.Math.Pow(10, 15), 6.2, MakeColor(208, 208, 192), 23463.2);

        //mars.AddSatellite(phobos);
        //mars.AddSatellite(deimos);

        // Jupiter
        Planet jupiter = MakePlanet("Jupiter", 317.8, 71492, MakeColor(207, 128, 89), 5.203);
        SetOrbitData(jupiter, 6.09, 100.464, 3.13, 9.9250 * Numbers.HourToSecond);

        // Saturn
        Planet saturn = MakePlanet("Saturn", 95.16, 60268, MakeColor(247, 200, 57), 9.539);
        SetOrbitData(saturn, 5.51, 113.665, 26.73, 10 * Numbers.HourToSecond + 33 * Numbers.MinuteToSecond + 38);

        // Uranus
        Planet uranus = MakePlanet("Uranus", 14.54, 25559, MakeColor(51, 191, 222), 19.18);
        SetOrbitData(uranus, 6.48, 74.006, 97.77 - 360, 0.71833 * Numbers.DayToSeconds);

        // Neptune
        Planet neptune = MakePlanet("Neptune", 17.15, 24764, MakeColor(6, 57, 199), 30.06);
        SetOrbitData(neptune, 6.43, 131.783, 28.32, 0.6713 * Numbers.DayToSeconds);

        // Pluto
        Planet pluto = MakePlanet("Pluto", 0.0022, 1195, MakeColor(186, 185, 182), 39.53);
        SetOrbitData(pluto, 11.88, 110.299, 122.53 - 360, 6.387230 * Numbers.DayToSeconds);

        sun.AddChild(mercury);
        sun.AddChild(venus);
        sun.AddChild(earth);
        sun.AddChild(mars);
        sun.AddChild(jupiter);
        sun.AddChild(saturn);
        sun.AddChild(uranus);
        sun.AddChild(neptune);
        sun.AddChild(pluto);


        // Terra Station
        OrbitalGrid terraStationGrid = new OrbitalGrid();
        terraStationGrid.name = "Terra Station";
        terraStationGrid.orbitRadius = earth.radius * 1.10;
        terraStationGrid.orbitPercentOffset = 0.37;

        SetOrbitData(terraStationGrid, 40, 32, 8, 20 * Numbers.MinuteToSecond);

        Station terraStation = new Station();
        terraStation.name = "Terra Station";
        terraStation.radiusM = 20000;
        // terraStation.position = new Vector3d(20000, 0, 0);
        // terraStation.rotation = Quaternion.Euler(-43, 0, 150);

        terraStationGrid.AddChild(terraStation);
        earth.AddChild(terraStationGrid);

        return sun;
    }
}
