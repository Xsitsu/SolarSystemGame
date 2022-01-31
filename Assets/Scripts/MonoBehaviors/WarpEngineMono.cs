using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpEngineMono : EngineMono
{
    [Range(1, 100)]
    public float maxWarp = 5.0f;
    [Range(1, 10)]
    public float acceleration = 0.5f;
    public Vector3 moveDirection;
    public float warpFactor { get; private set; }
    public float WarpFactorOut;
    public double speedCOut;
    public double speedAUOut;
    public double CalculateSpeedC()
    {
        double speedC = System.Math.Pow(warpFactor, 3);
        return speedC;
    }
    void Start()
    {
        warpFactor = 0;
        moveDirection = new Vector3(0, 0, 0);
    }

    void Update()
    {
        warpFactor += moveDirection.normalized.z * acceleration * Time.deltaTime;
        warpFactor = Mathf.Clamp(warpFactor, 0, maxWarp);
        WarpFactorOut = warpFactor;

        if (warpFactor > 0)
        {
            double speedC = CalculateSpeedC();
            double speedM = speedC * Numbers.c;
            double speedAU = (speedM / 1000) / Numbers.AUToKM;
            speedCOut = speedC;
            speedAUOut = speedAU;
            //transform.localPosition += transform.forward * (float)(speedM / Numbers.UnitsToMeters) * Time.deltaTime;
        }
        else
        {
            speedCOut = 0;
            speedAUOut = 0;
        }
    }

    public void ResetSpeed()
    {
        warpFactor = 0;
    }
}
