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
        double epoch_2000 = DateTimeToSeconds(new System.DateTime(2000, 1, 1));

        Star sun = new Star
        {
            name = "Sol",
            mass_kg = 1.000 * Numbers.SolarMassToKG,
            radius_m = 696340 * Numbers.KMToM,

            axialTilt_deg = 0,
            rotationalPeriod_sec =  25.05 * Numbers.DayToSeconds,

            color = MakeColor(252, 212, 64),
        };
        sun.SetOrbitData(0, 0, 0, 0, 0, 0);

        // Mercury
        Planet mercury = new Planet
        {
            name = "Mercury",
            mass_kg = 0.055 * Numbers.EarthMassToKGs,
            radius_m = 2439.7 * Numbers.KMToM,

            axialTilt_deg = 2.04,
            rotationalPeriod_sec =  58.646 * Numbers.DayToSeconds,

            color = MakeColor(128, 106, 75),
        };
        mercury.SetOrbitData(0.205, 0.387 * Numbers.AUToKM * Numbers.KMToM, 3.38, 48.331, 29.124, epoch_2000);

        // Venus
        Planet venus = new Planet
        {
            name = "Venus",
            mass_kg = 0.815 * Numbers.EarthMassToKGs,
            radius_m = 6051.8 * Numbers.KMToM,

            axialTilt_deg = 177.36,
            rotationalPeriod_sec =  243.0226 * Numbers.DayToSeconds,

            color = MakeColor(207, 184, 54),
        };
        venus.SetOrbitData(0.006, 0.723 * Numbers.AUToKM * Numbers.KMToM, 3.86, 76.680, 54.884, epoch_2000);

        // Earth
        Planet earth = new Planet
        {
            name = "Earth",
            mass_kg = 1.000 * Numbers.EarthMassToKGs,
            radius_m = 6371.0 * Numbers.KMToM,

            axialTilt_deg = 23.439,
            rotationalPeriod_sec = 23 * Numbers.HourToSecond + 56 * Numbers.MinuteToSecond + 4.100,

            color = MakeColor(49, 108, 196),
        };
        earth.SetOrbitData(0.016, 1.000 * Numbers.AUToKM * Numbers.KMToM, 7.155, -11.260, 114.207, epoch_2000);

        // Luna
        Planet luna = new Planet
        {
            name = "Luna",
            mass_kg = 0.0123 * Numbers.EarthMassToKGs,
            radius_m = 1737.4 * Numbers.KMToM,

            axialTilt_deg = 6.687,
            rotationalPeriod_sec = 27.321661 * Numbers.DayToSeconds,

            color = MakeColor(220, 220, 220),
        };
        luna.SetOrbitData(0.0549, 384339 * Numbers.KMToM, -23.439 + 5.145, 0, 0, epoch_2000);

        // earth.AddChild(luna);

        // Mars
        Planet mars = new Planet
        {
            name = "Mars",
            mass_kg = 0.107 * Numbers.EarthMassToKGs,
            radius_m = 3389.5 * Numbers.KMToM,

            axialTilt_deg = 25.19,
            rotationalPeriod_sec = 1.025957 * Numbers.DayToSeconds,

            color = MakeColor(189, 66, 28),
        };
        mars.SetOrbitData(0.0934, 1.523 * Numbers.AUToKM * Numbers.KMToM, 5.65, 49.558, 286.502, epoch_2000);

        // Jupiter
        Planet jupiter = new Planet
        {
            name = "Jupiter",
            mass_kg = 317.8 * Numbers.EarthMassToKGs,
            radius_m = 69911 * Numbers.KMToM,

            axialTilt_deg = 3.13,
            rotationalPeriod_sec = 9 * Numbers.HourToSecond + 55 * Numbers.MinuteToSecond + 30,

            color = MakeColor(207, 128, 89),
        };
        jupiter.SetOrbitData(0.0489, 5.2044 * Numbers.AUToKM * Numbers.KMToM, 6.09, 100.464, 273.867, epoch_2000);

        // Saturn
        Planet saturn = new Planet
        {
            name = "Saturn",
            mass_kg = 95.159 * Numbers.EarthMassToKGs,
            radius_m = 58232 * Numbers.KMToM,

            axialTilt_deg = 26.73,
            rotationalPeriod_sec = 10 * Numbers.HourToSecond + 33 * Numbers.MinuteToSecond + 38,

            color = MakeColor(247, 200, 57),
        };
        saturn.SetOrbitData(0.0565, 9.5826 * Numbers.AUToKM * Numbers.KMToM, 5.51, 113.665, 339.392, epoch_2000);

        Ring saturnRing = new Ring
        {
            name = "Saturn's Rings",
            mass_kg = 1,
            radius_m = (270000 / 2) * Numbers.KMToM,
        };

        // Uranus
        Planet uranus = new Planet
        {
            name = "Uranus",
            mass_kg = 14.536 * Numbers.EarthMassToKGs,
            radius_m = 25362 * Numbers.KMToM,

            axialTilt_deg = 97.77 - 360,
            rotationalPeriod_sec = 17 * Numbers.HourToSecond + 14 * Numbers.MinuteToSecond + 24,

            color = MakeColor(51, 191, 222),
        };
        uranus.SetOrbitData(0.047, 19.191 * Numbers.AUToKM * Numbers.KMToM, 6.48, 74.006, 96.998, epoch_2000);

        // Neptune
        Planet neptune = new Planet
        {
            name = "Neptune",
            mass_kg = 17.147 * Numbers.EarthMassToKGs,
            radius_m = 24622 * Numbers.KMToM,

            axialTilt_deg = 28.32,
            rotationalPeriod_sec = 16 * Numbers.HourToSecond + 6 * Numbers.MinuteToSecond + 36,

            color = MakeColor(6, 57, 199),
        };
        neptune.SetOrbitData(0.008, 30.07 * Numbers.AUToKM * Numbers.KMToM, 6.43, 131.783, 273.187, epoch_2000);

        // Pluto
        Planet pluto = new Planet
        {
            name = "Pluto",
            mass_kg = 0.00218 * Numbers.EarthMassToKGs,
            radius_m = 1188.3 * Numbers.KMToM,

            axialTilt_deg = 122.53 - 360,
            rotationalPeriod_sec = 6 * Numbers.DayToSeconds + 9 * Numbers.HourToSecond + 17 * Numbers.MinuteToSecond + 0,

            color = MakeColor(186, 185, 182),
        };
        pluto.SetOrbitData(0.2488, 39.482 * Numbers.AUToKM * Numbers.KMToM, 11.88, 110.299, 113.834, epoch_2000);

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
        OrbitalGrid terraStationGrid = new OrbitalGrid();
        terraStationGrid.name = "Terra Station Grid";
        terraStationGrid.SetOrbitData(0.0006822, 6738 * Numbers.KMToM, 51.6445, 292.2902, 83.2131, DateTimeToSeconds(new System.DateTime(2022, 01, 31, 17, 50, 36)));

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
        terraStationGrid = new OrbitalGrid();
        terraStationGrid.name = "Terra Station 2 Grid";
        terraStationGrid.SetOrbitData(0.0006822, 7238 * Numbers.KMToM, 0, 0, 0, DateTimeToSeconds(new System.DateTime(2022, 01, 31, 17, 50, 36)));

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
        OrbitalGrid lunaStationGrid = new OrbitalGrid();
        lunaStationGrid.name = "Luna Station Grid";
        lunaStationGrid.SetOrbitData(0.0006822, luna.radius_m * 2, 22.6445, 79.2902, 20.2131, DateTimeToSeconds(new System.DateTime(2022, 01, 31, 17, 50, 36)));

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
        OrbitalGrid station = new OrbitalGrid();
        station.name = "Mars Station";
        station.SetOrbitData(0, mars.radius_m * 1.1, -18.58, -88.7, 20.2131, DateTimeToSeconds(new System.DateTime(2022, 01, 31, 17, 50, 36)));
        mars.AddChild(StationHelper(station));

        // Jupiter Station
        station = new OrbitalGrid();
        station.name = "Jupiter Station";
        station.SetOrbitData(0, jupiter.radius_m * 1.4, 32.086, -22.7, 20.2131, DateTimeToSeconds(new System.DateTime(2022, 01, 31, 17, 50, 36)));
        jupiter.AddChild(StationHelper(station));

        station = new OrbitalGrid();
        station.name = "Jupiter Station 2";
        station.SetOrbitData(0, jupiter.radius_m + (200 * Numbers.KMToM), 78.086, 28, 12.2131, DateTimeToSeconds(new System.DateTime(2022, 01, 31, 17, 50, 36)));
        jupiter.AddChild(StationHelper(station));

        // Saturn Station
        station = new OrbitalGrid();
        station.name = "Saturn Station";
        station.SetOrbitData(0, saturn.radius_m * 1.67, 8, 36.2, 28, DateTimeToSeconds(new System.DateTime(2022, 01, 31, 17, 50, 36)));
        saturn.AddChild(StationHelper(station));

        station = new OrbitalGrid();
        station.name = "Saturn Station 2";
        station.SetOrbitData(0, saturn.radius_m * 1.42, 68, 2.2, 88, DateTimeToSeconds(new System.DateTime(2022, 01, 31, 17, 50, 36)));
        saturn.AddChild(StationHelper(station));

        return sun;
    }
}
