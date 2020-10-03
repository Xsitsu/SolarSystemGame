using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Numbers
{
    public static double G = 6.673 * System.Math.Pow(10, -11); // m^2 / kg^2
    public static double YearToSeconds = 60 * 60 * 24 * 365.25;
    public static double DayToSeconds = 60 * 60 * 24;
    public static double AUToKM = 149597870;
    public static double UnitsToMeters = 100; // Unity spatial coordinate system to meters
    public static double SunMassToKGs = 1.989 * System.Math.Pow(10, 30);
    public static double EarthMassToKGs = 5.972 * System.Math.Pow(10, 24);
    public static double PI2 = System.Math.PI * 2;
    public static double PISquared = System.Math.PI * System.Math.PI;
}
