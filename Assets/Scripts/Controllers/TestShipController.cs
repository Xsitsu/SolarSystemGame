using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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


    private ShipActions shipActions;

    
    Quaternion handRotationBase;
    bool isQuatRot = false;

    void OnEnable()
    {
        shipActions.Enable();
    }
    void OnDisable()
    {
        shipActions.Disable();
    }
    void Awake()
    {
        sublightEngine = GetComponent<SublightEngineMono>();
        warpEngine = GetComponent<WarpEngineMono>();

        shipActions = new ShipActions();
    }
    void Start()
    {
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
        warpEngine.moveDirection.z = shipActions.Player.Accelerate.ReadValue<float>();
    }

    void UpdateSublight()
    {
        float sublightAcceleration = shipActions.Player.Accelerate.ReadValue<float>();
        Vector3 sublightRotation = new Vector3(0, 0, 0);

        if (autoBrake && sublightAcceleration < 0.01f)
        {
            sublightAcceleration = -1;
        }

        sublightRotation.x = shipActions.Player.Pitch.ReadValue<float>();
        sublightRotation.y = shipActions.Player.Yaw.ReadValue<float>();
        sublightRotation.z = shipActions.Player.Roll.ReadValue<float>();

        sublightEngine.moveDirection.z = sublightAcceleration;
        sublightEngine.rotateDirection = sublightRotation;

        if (shipActions.Player.ShipRotationEnable.ReadValue<float>() > 0.5f)
        {
            sublightEngine.useRotateQuaternion = true;

            if (!isQuatRot)
            {
                isQuatRot = true;
                handRotationBase = Quaternion.Inverse(shipActions.Player.ShipRotation.ReadValue<Quaternion>());
            }
            else
            {
                sublightEngine.rotateQuaternion = shipActions.Player.ShipRotation.ReadValue<Quaternion>() * handRotationBase;
            }
        }
        else
        {
            isQuatRot = false;
            sublightEngine.useRotateQuaternion = false;
            sublightEngine.rotateQuaternion = shipActions.Player.ShipRotation.ReadValue<Quaternion>();
        }
    }
}
