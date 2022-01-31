using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShipController : MonoBehaviour
{
    public bool autoBrake;
    public Vector3 sizeMeters = new Vector3(20, 20, 20);
    public GameObject DirectionLine;
    public GameObject WarpTargetLine;

    LineRenderer _directionLine;
    LineRenderer _warpTargetLine;

    SublightEngineMono sublightEngine;
    WarpEngineMono warpEngine;
    void Start()
    {
        sublightEngine = GetComponent<SublightEngineMono>();
        warpEngine = GetComponent<WarpEngineMono>();
        transform.localScale = sizeMeters / (float)Numbers.UnitsToMeters;

        _directionLine = DirectionLine.GetComponent<LineRenderer>();
        _warpTargetLine = WarpTargetLine.GetComponent<LineRenderer>();
    }

    string GetDistText(double distM)
    {
        string _distanceText = "";

        double distKM = distM / 1000d;
        double distAU = distKM / Numbers.AUToKM;
        double distLY = distKM / Numbers.LightYearToKM;

        if (distLY >= 0.1)
        {
            _distanceText = (System.Math.Round(distLY, 1).ToString() + " ly");
        }
        else if (distAU >= 0.1)
        {
            _distanceText = (System.Math.Round(distAU, 1).ToString() + " au");
        }
        else if (distKM >= 10)
        {
            _distanceText = (System.Math.Round(distKM, 0).ToString() + " km");
        }
        else if (distM > 0)
        {
            _distanceText = (System.Math.Round(distM, 0).ToString() + " m");
        }
        else
        {
            _distanceText = "";
        }

        return _distanceText;
    }
    void Update()
    {
        if (enabled)
        {
            Warp warp = WarpManager.Instance.GetWarp(PlayerManager.Instance.CharacterStructure);
            if (warp != null)
            {
                warp.primaryEngine = warpEngine;
                UpdateWarp();

                double currentTime_sec = StarSystemDisplay.Instance.CalculateCurrentTime();
                Vector3d posTo = warp.GetPositionToTarget(currentTime_sec);

                _directionLine.enabled = true;
                _warpTargetLine.enabled = true;

                _directionLine.SetPosition(0, transform.position);
                _warpTargetLine.SetPosition(0, transform.position);

                warp.UpdateWarpLines(_directionLine, _warpTargetLine, transform.position);

                // Vector3d posUnits = posTo / Numbers.UnitsToMeters;
                // Debug.LogFormat("Has warp to target {0}. Pos: {1}, Dist: {2}", warp.target.name, posUnits.ToUnity(), GetDistText(posTo.magnitude));
            }
            else
            {
                UpdateSublight();

                _directionLine.enabled = false;
                _warpTargetLine.enabled = false;
            }
        }
    }

    void UpdateWarp()
    {
        int warpAcceleration = 0;
        if (Input.GetKey(KeyCode.W))
        {
            warpAcceleration = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            warpAcceleration = -1;
        }

        warpEngine.moveDirection.z = warpAcceleration;
    }

    void UpdateSublight()
    {
        int sublightAcceleration = 0;
        Vector3 sublightRotation = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            sublightAcceleration = 1;
        }
        else if (Input.GetKey(KeyCode.S) || autoBrake)
        {
            sublightAcceleration = -1;
        }

        int rotX = 0;
        int rotY = 0;
        int rotZ = 0;
        if (Input.GetKey(KeyCode.Q)) rotZ++;
        if (Input.GetKey(KeyCode.E)) rotZ--;
        if (Input.GetKey(KeyCode.A)) rotY--;
        if (Input.GetKey(KeyCode.D)) rotY++;
        if (Input.GetKey(KeyCode.Z)) rotX--;
        if (Input.GetKey(KeyCode.C)) rotX++;

        sublightRotation.x = rotX;
        sublightRotation.y = rotY;
        sublightRotation.z = rotZ;

        sublightEngine.moveDirection.z = sublightAcceleration;
        sublightEngine.rotateDirection = sublightRotation;
    }
}
