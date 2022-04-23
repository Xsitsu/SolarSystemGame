using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemGeneratorSol : ISystemGenerator
{
    static System.DateTime timeEpoch = new System.DateTime(1970, 1, 1, 0, 0, 0);
    Color MakeColor(int r, int g, int b)
    {
        return new Color(r / 255f, g / 255f, b / 255f, 1);
    }

    double DateTimeToSeconds(System.DateTime dt)
    {
        return (dt.Subtract(timeEpoch)).TotalSeconds;
    }

    OrbitalGrid StationHelper(OrbitalGrid grid)
    {
        Station makeStation = new Station()
        {
            name = grid.name,

            radius_m = 10000,

            rotationSpeed_degPersec = new Vector3d(0, 2, 0),
        };
        grid.name += " Station";
        grid.AddChild(makeStation);
        return grid;
    }

    public Star Generate()
    {
        Star sun = new Star
        {
            name = "Sol",
            mass_kg = 1.000 * Numbers.SolarMassToKG,
            radius_m = 696340 * Numbers.KMToM,

            eccentricity = 0,
            semiMajorAxis_m = 0,
            inclination_deg = 0,
            longitudeOfAN_deg = 0,
            argumentOfPeriapsis_deg = 0,
            periapsisEpoch_sec = 0,

            axialTilt_deg = 0,
            rotationalPeriod_sec =  25.05 * Numbers.DayToSeconds,

            color = MakeColor(252, 212, 64),
        };

        // Mercury
        Planet mercury = new Planet
        {
            name = "Mercury",
            mass_kg = 0.055 * Numbers.EarthMassToKGs,
            radius_m = 2439.7 * Numbers.KMToM,

            eccentricity = 0.205,
            semiMajorAxis_m = 0.387 * Numbers.AUToKM * Numbers.KMToM,
            inclination_deg = 3.38,
            longitudeOfAN_deg = 48.331,
            argumentOfPeriapsis_deg = 29.124,
            periapsisEpoch_sec = DateTimeToSeconds(new System.DateTime(2000, 1, 1)),

            axialTilt_deg = 2.04,
            rotationalPeriod_sec =  58.646 * Numbers.DayToSeconds,

            color = MakeColor(128, 106, 75),
        };

        // Venus
        Planet venus = new Planet
        {
            name = "Venus",
            mass_kg = 0.815 * Numbers.EarthMassToKGs,
            radius_m = 6051.8 * Numbers.KMToM,

            eccentricity = 0.006,
            semiMajorAxis_m = 0.723 * Numbers.AUToKM * Numbers.KMToM,
            inclination_deg = 3.86,
            longitudeOfAN_deg = 76.680,
            argumentOfPeriapsis_deg = 54.884,
            periapsisEpoch_sec = DateTimeToSeconds(new System.DateTime(2000, 1, 1)),

            axialTilt_deg = 177.36,
            rotationalPeriod_sec =  243.0226 * Numbers.DayToSeconds,

            color = MakeColor(207, 184, 54),
        };

        // Earth
        Planet earth = new Planet
        {
            name = "Earth",
            mass_kg = 1.000 * Numbers.EarthMassToKGs,
            radius_m = 6371.0 * Numbers.KMToM,

            eccentricity = 0.016,
            semiMajorAxis_m = 1.000 * Numbers.AUToKM * Numbers.KMToM,
            inclination_deg = 7.155,
            longitudeOfAN_deg = -11.260,
            argumentOfPeriapsis_deg = 114.207,
            periapsisEpoch_sec = DateTimeToSeconds(new System.DateTime(2000, 1, 1)),

            axialTilt_deg = 23.439,
            rotationalPeriod_sec = 23 * Numbers.HourToSecond + 56 * Numbers.MinuteToSecond + 4.100,

            color = MakeColor(49, 108, 196),
        };

        // Luna
        Planet luna = new Planet
        {
            name = "Luna",
            mass_kg = 0.0123 * Numbers.EarthMassToKGs,
            radius_m = 1737.4 * Numbers.KMToM,

            eccentricity = 0.0549,
            semiMajorAxis_m = 384339 * Numbers.KMToM,
            inclination_deg = -23.439 + 5.145,
            longitudeOfAN_deg = 0,
            argumentOfPeriapsis_deg = 0,
            periapsisEpoch_sec = DateTimeToSeconds(new System.DateTime(2000, 1, 1)),

            axialTilt_deg = 6.687,
            rotationalPeriod_sec = 27.321661 * Numbers.DayToSeconds,

            color = MakeColor(220, 220, 220),
        };

        // earth.AddChild(luna);

        // Mars
        Planet mars = new Planet
        {
            name = "Mars",
            mass_kg = 0.107 * Numbers.EarthMassToKGs,
            radius_m = 3389.5 * Numbers.KMToM,

            eccentricity = 0.0934,
            semiMajorAxis_m = 1.523 * Numbers.AUToKM * Numbers.KMToM,
            inclination_deg = 5.65,
            longitudeOfAN_deg = 49.558,
            argumentOfPeriapsis_deg = 286.502,
            periapsisEpoch_sec = DateTimeToSeconds(new System.DateTime(2000, 1, 1)),

            axialTilt_deg = 25.19,
            rotationalPeriod_sec = 1.025957 * Numbers.DayToSeconds,

            color = MakeColor(189, 66, 28),
        };

        // Jupiter
        Planet jupiter = new Planet
        {
            name = "Jupiter",
            mass_kg = 317.8 * Numbers.EarthMassToKGs,
            radius_m = 69911 * Numbers.KMToM,

            eccentricity = 0.0489,
            semiMajorAxis_m = 5.2044 * Numbers.AUToKM * Numbers.KMToM,
            inclination_deg = 6.09,
            longitudeOfAN_deg = 100.464,
            argumentOfPeriapsis_deg = 273.867,
            periapsisEpoch_sec = DateTimeToSeconds(new System.DateTime(2000, 1, 1)),

            axialTilt_deg = 3.13,
            rotationalPeriod_sec = 9 * Numbers.HourToSecond + 55 * Numbers.MinuteToSecond + 30,

            color = MakeColor(207, 128, 89),
        };

        // Saturn
        Planet saturn = new Planet
        {
            name = "Saturn",
            mass_kg = 95.159 * Numbers.EarthMassToKGs,
            radius_m = 58232 * Numbers.KMToM,

            eccentricity = 0.0565,
            semiMajorAxis_m = 9.5826 * Numbers.AUToKM * Numbers.KMToM,
            inclination_deg = 5.51,
            longitudeOfAN_deg = 113.665,
            argumentOfPeriapsis_deg = 339.392,
            periapsisEpoch_sec = DateTimeToSeconds(new System.DateTime(2000, 1, 1)),

            axialTilt_deg = 26.73,
            rotationalPeriod_sec = 10 * Numbers.HourToSecond + 33 * Numbers.MinuteToSecond + 38,

            color = MakeColor(247, 200, 57),
        };

        Ring saturnRing = new Ring
        {
            name = "Saturn's Rings",
            mass_kg = 1,
            radius_m = (270000 / 2) * Numbers.KMToM,

            eccentricity = 0,
            semiMajorAxis_m = 0,
            inclination_deg = 0,
            longitudeOfAN_deg = 0,
            argumentOfPeriapsis_deg = 0,
            periapsisEpoch_sec = 0
        };

        // Uranus
        Planet uranus = new Planet
        {
            name = "Uranus",
            mass_kg = 14.536 * Numbers.EarthMassToKGs,
            radius_m = 25362 * Numbers.KMToM,

            eccentricity = 0.047,
            semiMajorAxis_m = 19.191 * Numbers.AUToKM * Numbers.KMToM,
            inclination_deg = 6.48,
            longitudeOfAN_deg = 74.006,
            argumentOfPeriapsis_deg = 96.998,
            periapsisEpoch_sec = DateTimeToSeconds(new System.DateTime(2000, 1, 1)),

            axialTilt_deg = 97.77 - 360,
            rotationalPeriod_sec = 17 * Numbers.HourToSecond + 14 * Numbers.MinuteToSecond + 24,

            color = MakeColor(51, 191, 222),
        };

        // Neptune
        Planet neptune = new Planet
        {
            name = "Neptune",
            mass_kg = 17.147 * Numbers.EarthMassToKGs,
            radius_m = 24622 * Numbers.KMToM,

            eccentricity = 0.008,
            semiMajorAxis_m = 30.07 * Numbers.AUToKM * Numbers.KMToM,
            inclination_deg = 6.43,
            longitudeOfAN_deg = 131.783,
            argumentOfPeriapsis_deg = 273.187,
            periapsisEpoch_sec = DateTimeToSeconds(new System.DateTime(2000, 1, 1)),

            axialTilt_deg = 28.32,
            rotationalPeriod_sec = 16 * Numbers.HourToSecond + 6 * Numbers.MinuteToSecond + 36,

            color = MakeColor(6, 57, 199),
        };

        // Pluto
        Planet pluto = new Planet
        {
            name = "Pluto",
            mass_kg = 0.00218 * Numbers.EarthMassToKGs,
            radius_m = 1188.3 * Numbers.KMToM,

            eccentricity = 0.2488,
            semiMajorAxis_m = 39.482 * Numbers.AUToKM * Numbers.KMToM,
            inclination_deg = 11.88,
            longitudeOfAN_deg = 110.299,
            argumentOfPeriapsis_deg = 113.834,
            periapsisEpoch_sec = DateTimeToSeconds(new System.DateTime(2000, 1, 1)),

            axialTilt_deg = 122.53 - 360,
            rotationalPeriod_sec = 6 * Numbers.DayToSeconds + 9 * Numbers.HourToSecond + 17 * Numbers.MinuteToSecond + 0,

            color = MakeColor(186, 185, 182),
        };

        sun.AddChild(mercury);
        sun.AddChild(venus);
        sun.AddChild(earth);
        sun.AddChild(mars);
        sun.AddChild(jupiter);
        sun.AddChild(saturn);
        sun.AddChild(uranus);
        sun.AddChild(neptune);
        sun.AddChild(pluto);

        earth.AddChild(luna);

        saturn.AddChild(saturnRing);

        // Terra Station
        OrbitalGrid terraStationGrid = new OrbitalGrid
        {
            name = "Terra Station Grid",

            eccentricity = 0.0006822,
            semiMajorAxis_m = 6738 * Numbers.KMToM,
            inclination_deg = 51.6445,
            longitudeOfAN_deg = 292.2902,
            argumentOfPeriapsis_deg = 83.2131,
            periapsisEpoch_sec = DateTimeToSeconds(new System.DateTime(2022, 01, 31, 17, 50, 36)),
        };

        Station terraStation = new Station()
        {
            name = "Terra Station",

            radius_m = 10000,
            // position = new Vector3d(0, 20000, 0),

            rotationSpeed_degPersec = new Vector3d(0, 2, 0),
        };

        terraStationGrid.AddChild(terraStation);
        earth.AddChild(terraStationGrid);


        // Terra Station 2
        terraStationGrid = new OrbitalGrid
        {
            name = "Terra Station 2 Grid",

            eccentricity = 0.0006822,
            semiMajorAxis_m = 7238 * Numbers.KMToM,
            inclination_deg = 0,
            longitudeOfAN_deg = 0,
            argumentOfPeriapsis_deg = 0,
            periapsisEpoch_sec = DateTimeToSeconds(new System.DateTime(2022, 01, 31, 17, 50, 36)),
        };

        terraStation = new Station()
        {
            name = "Terra Station 2",

            radius_m = 10000,
            // position = new Vector3d(0, 20000, 0),

            rotationSpeed_degPersec = new Vector3d(0, 2, 0),
        };

        terraStationGrid.AddChild(terraStation);
        earth.AddChild(terraStationGrid);


        // Luna Station
        OrbitalGrid lunaStationGrid = new OrbitalGrid
        {
            name = "Luna Station Grid",

            eccentricity = 0.0006822,
            semiMajorAxis_m = luna.radius_m * 2,
            inclination_deg = 22.6445,
            longitudeOfAN_deg = 79.2902,
            argumentOfPeriapsis_deg = 20.2131,
            periapsisEpoch_sec = DateTimeToSeconds(new System.DateTime(2022, 01, 31, 17, 50, 36)),
        };

        Station lunaStation = new Station()
        {
            name = "Luna Station",

            radius_m = 10000,
            // position = new Vector3d(0, 20000, 0),

            rotationSpeed_degPersec = new Vector3d(0, 2, 0),
        };

        lunaStationGrid.AddChild(lunaStation);
        luna.AddChild(lunaStationGrid);

        // Mars Station
        mars.AddChild(StationHelper(new OrbitalGrid
        {
            name = "Mars Station",

            eccentricity = 0,
            semiMajorAxis_m = mars.radius_m * 1.1,
            inclination_deg = -18.58,
            longitudeOfAN_deg = -88.7,
            argumentOfPeriapsis_deg = 20.2131,
            periapsisEpoch_sec = DateTimeToSeconds(new System.DateTime(2022, 01, 31, 17, 50, 36)),
        }));

        // Jupiter Station
        jupiter.AddChild(StationHelper(new OrbitalGrid
        {
            name = "Jupiter Station",

            eccentricity = 0,
            semiMajorAxis_m = jupiter.radius_m * 1.4,
            inclination_deg = 32.086,
            longitudeOfAN_deg = -22.7,
            argumentOfPeriapsis_deg = 20.2131,
            periapsisEpoch_sec = DateTimeToSeconds(new System.DateTime(2022, 01, 31, 17, 50, 36)),
        }));

        jupiter.AddChild(StationHelper(new OrbitalGrid
        {
            name = "Jupiter Station 2",

            eccentricity = 0,
            semiMajorAxis_m = jupiter.radius_m + (200 * Numbers.KMToM),
            inclination_deg = 78.086,
            longitudeOfAN_deg = 28,
            argumentOfPeriapsis_deg = 12.2131,
            periapsisEpoch_sec = DateTimeToSeconds(new System.DateTime(2022, 01, 31, 17, 50, 36)),
        }));

        // Saturn Station
        saturn.AddChild(StationHelper(new OrbitalGrid
        {
            name = "Saturn Station",

            eccentricity = 0,
            semiMajorAxis_m = saturn.radius_m * 1.67,
            inclination_deg = 8,
            longitudeOfAN_deg = 36.2,
            argumentOfPeriapsis_deg = 28,
            periapsisEpoch_sec = DateTimeToSeconds(new System.DateTime(2022, 01, 31, 17, 50, 36)),
        }));

        saturn.AddChild(StationHelper(new OrbitalGrid
        {
            name = "Saturn Station 2",

            eccentricity = 0,
            semiMajorAxis_m = saturn.radius_m * 1.42,
            inclination_deg = 68,
            longitudeOfAN_deg = 2.2,
            argumentOfPeriapsis_deg = 88,
            periapsisEpoch_sec = DateTimeToSeconds(new System.DateTime(2022, 01, 31, 17, 50, 36)),
        }));

        return sun;
    }
}
