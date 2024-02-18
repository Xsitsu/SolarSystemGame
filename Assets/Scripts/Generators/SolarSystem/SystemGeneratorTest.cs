using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemGeneratorTest : ISystemGenerator
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

    public Star Generate()
    {
        Star sun = new Star
        {
            name = "Sol",
            mass_kg = 1,
            // mass_kg = 0.01 * Numbers.EarthMassToKGs,
            radius_m = 1000 * Numbers.KMToM,

            axialTilt_deg = 0,
            rotationalPeriod_sec =  60,

            color = MakeColor(252, 212, 64),
        };
        sun.SetOrbitData(0, 0, 0, 0, 0, 0);

        // Red
        Planet red = new Planet
        {
            name = "Red",
            mass_kg = 1,
            radius_m = 100 * Numbers.KMToM,

            axialTilt_deg = 0,
            rotationalPeriod_sec = 23 * Numbers.HourToSecond + 56 * Numbers.MinuteToSecond + 4.100,

            color = MakeColor(255, 0, 0),
        };
        red.SetOrbitData(0, 2000 * Numbers.KMToM, 0, 0, 0, 0);

        // Blue
        Planet blue = new Planet
        {
            name = "Blue",
            mass_kg = 1,
            radius_m = 100 * Numbers.KMToM,

            axialTilt_deg = 0,
            rotationalPeriod_sec = 23 * Numbers.HourToSecond + 56 * Numbers.MinuteToSecond + 4.100,

            color = MakeColor(0, 0, 255),
        };
        blue.SetOrbitData(0, 2000 * Numbers.KMToM , 0, 90, 0, 0);




        sun.AddChild(red);
        sun.AddChild(blue);
        



        // Red Station
        OrbitalGrid redStationGrid = new OrbitalGrid();
        redStationGrid.name = "Red Station";
        redStationGrid.SetOrbitData(0, 200 * Numbers.KMToM, 0, 0, 0, 0);

        Station redStation = new Station()
        {
            name = "Red Station",

            radius_m = 10000,
            // position = new Vector3d(0, 20000, 0),

            rotationSpeed_degPersec = new Vector3d(0, 2, 0),
        };

        redStationGrid.AddChild(redStation);
        red.AddChild(redStationGrid);


        // Blue Station
        OrbitalGrid blueStationGrid = new OrbitalGrid();
        blueStationGrid.name = "Blue Station";
        blueStationGrid.SetOrbitData(0, 200 * Numbers.KMToM, 0, 0, 0, 0);

        Station blueStation = new Station()
        {
            name = "Blue Station",

            radius_m = 10000,
            // position = new Vector3d(0, 20000, 0),

            rotationSpeed_degPersec = new Vector3d(0, 2, 0),
        };

        blueStationGrid.AddChild(blueStation);
        blue.AddChild(blueStationGrid);




        // Sun Station
        OrbitalGrid sunStationGrid = new OrbitalGrid();
        sunStationGrid.name = "Sol Station";
        sunStationGrid.SetOrbitData(0, 1500 * Numbers.KMToM, 70, 22, 50, 0);

        Station sunStation = new Station()
        {
            name = "Sol Station",

            radius_m = 10000,
            // position = new Vector3d(0, 20000, 0),

            rotationSpeed_degPersec = new Vector3d(0, 2, 0),
        };

        sunStationGrid.AddChild(sunStation);
        sun.AddChild(sunStationGrid);




        // Sun Station 2
        sunStationGrid = new OrbitalGrid();
        sunStationGrid.name = "Sol Station 2";
        sunStationGrid.SetOrbitData(0, 1500 * Numbers.KMToM, 0, 0, 180, 0);

        sunStation = new Station()
        {
            name = "Sol Station 2",

            radius_m = 10000,
            // position = new Vector3d(0, 20000, 0),

            rotationSpeed_degPersec = new Vector3d(0, 2, 0),
        };

        sunStationGrid.AddChild(sunStation);
        sun.AddChild(sunStationGrid);

        return sun;
    }
}
