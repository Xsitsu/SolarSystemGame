using System;
using System.Collections.Generic;
using System.Text;

public static class Numbers
{
    // Game Specific
    public static double UnitsToMeters = 100;

    // Constants
    public static double G = 6.673 * System.Math.Pow(10, -11);                  // (N*m)^2 / kg^2
    public static double c = 299792458;                                         // m/s
    public static double PI2 = System.Math.PI * 2;
    public static double PISquared = System.Math.PI * System.Math.PI;

    // Time
    public static double YearToSeconds = 60 * 60 * 24 * 365.25;
    public static double DayToSeconds = 60 * 60 * 24;
    public static double HourToSecond = 60 * 60;
    public static double MinuteToSecond = 60;

    // Distance
    public static double LightYearToKM = 9460730472580.8;
    public static double AUToKM = 149597870;
    public static double KMToM = 1000;

    // Mass
    public static double SolarMassToKG = 1.989 * System.Math.Pow(10, 30);
    public static double EarthMassToKGs = 5.972 * System.Math.Pow(10, 24);

}

