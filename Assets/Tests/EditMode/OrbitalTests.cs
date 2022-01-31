using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class OrbitalTests
{
    static double wiggleRoomPercent = 0.0000001;

    public void VerifyResult(Vector3d expected, Vector3d actual)
    {
        double wiggleRoom = actual.magnitude * wiggleRoomPercent;

        Vector3d result = actual - expected;
        double mag = result.magnitude;
        double magPercent = (mag / actual.magnitude) * 100;
        string msg = string.Format("Object position had variance of {0} or {1}%.\nExpected: ({2})\nActual: ({3})", mag, magPercent, expected, actual);
        Assert.IsTrue(wiggleRoom > mag, msg);
    }

    [Test]
    public void RedStartingPosition()
    {
        double testTime = 0;
        Star star = new SystemGeneratorTest().Generate();
        Planet planet = (Planet)star.children[0];

        Vector3d expectedPosition = new Vector3d(0, 0, planet.semiMajorAxis_m);
        Vector3d actualPosition = planet.CalculatePosition(testTime);
        VerifyResult(expectedPosition, actualPosition);
    }
    [Test]
    public void RedStationPosition()
    {
        double testTime = 0;
        Star star = new SystemGeneratorTest().Generate();
        Planet planet = (Planet)star.children[0];
        OrbitalGrid station = (OrbitalGrid)planet.children[0];

        Vector3d expectedPosition = new Vector3d(0, 0, station.semiMajorAxis_m);
        Vector3d actualPosition = station.CalculatePosition(testTime);
        VerifyResult(expectedPosition, actualPosition);
    }
    [Test]
    public void RedStationPositionRelativeToSun()
    {
        double testTime = 0;
        Star star = new SystemGeneratorTest().Generate();
        Planet planet = (Planet)star.children[0];
        OrbitalGrid station = (OrbitalGrid)planet.children[0];

        Vector3d expectedPosition = new Vector3d(0, 0, station.semiMajorAxis_m + planet.semiMajorAxis_m);
        Vector3d actualPosition = station.CalculatePosition_Relative(testTime, star);
        VerifyResult(expectedPosition, actualPosition);
    }

    [Test]
    public void BlueStartingPosition()
    {
        double testTime = 0;
        Star star = new SystemGeneratorTest().Generate();
        Planet planet = (Planet)star.children[1];

        Vector3d expectedPosition = new Vector3d(-planet.semiMajorAxis_m, 0, 0);
        Vector3d actualPosition = planet.CalculatePosition(testTime);
        VerifyResult(expectedPosition, actualPosition);
    }
    [Test]
    public void BlueStationPosition()
    {
        double testTime = 0;
        Star star = new SystemGeneratorTest().Generate();
        Planet planet = (Planet)star.children[1];
        OrbitalGrid station = (OrbitalGrid)planet.children[0];

        Vector3d expectedPosition = new Vector3d(0, 0, station.semiMajorAxis_m);
        Vector3d actualPosition = station.CalculatePosition(testTime);
        VerifyResult(expectedPosition, actualPosition);
    }
    [Test]
    public void BlueStationPositionRelativeToSun()
    {
        double testTime = 0;
        Star star = new SystemGeneratorTest().Generate();
        Planet planet = (Planet)star.children[1];
        OrbitalGrid station = (OrbitalGrid)planet.children[0];

        Vector3d expectedPosition = new Vector3d(-(station.semiMajorAxis_m + planet.semiMajorAxis_m), 0, 0);
        Vector3d actualPosition = station.CalculatePosition_Relative(testTime, star);
        VerifyResult(expectedPosition, actualPosition);
    }
}
