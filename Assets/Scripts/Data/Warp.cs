using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp
{
    static double warpThresh_m = 0.1 * Numbers.AUToKM * Numbers.KMToM;
    public WarpEngineMono primaryEngine;

    public Structure warpee;
    public Entity target;

    StationaryGrid _warpGrid;
    OrbitalGrid _fromGrid;
    WarpState _state = new WarpState_Init();

    bool _isFinished = false;
    double _distance = 0;
    Vector3d _positionOffset = new Vector3d(0, 0, 0);

    public void Update(float deltaTime_sec, double currentTime_sec)
    {
        _state.Update(this, deltaTime_sec, currentTime_sec);
    }
    public bool IsFinished()
    {
        return _isFinished;
    }
    public Vector3d GetPositionToTarget(double currentTime_sec)
    {
        return _warpGrid.CalculatePosition_Relative(currentTime_sec, target);
    }
    public Vector3 GetVectorTowardsTarget(double currentTime_sec)
    {
        Entity common = _warpGrid.FindCommonAncestor(target);
        Quaternion rot_relCommon = _warpGrid.CalculateRotation_Relative(currentTime_sec, common);

        Vector3d posTo = GetPositionToTarget(currentTime_sec);
        Vector3 usePos = Quaternion.Inverse(rot_relCommon) * posTo.ToUnity();
        return usePos;
    }
    public void UpdateWarpLines(LineRenderer forwardLine, LineRenderer warpLine, Vector3 originPoint)
    {
        double currentTime_sec = StarSystemDisplay.Instance.CalculateCurrentTime();
        Vector3 usePos = GetVectorTowardsTarget(currentTime_sec);

        Vector3 frontLine_forward = Vector3.forward;
        Vector3 warpLine_forward = usePos;

        forwardLine.SetPosition(1, originPoint + (frontLine_forward * 10000));
        warpLine.SetPosition(1, originPoint - Vector3.Normalize(warpLine_forward));
    }
    void UpdatePosition(double deltaTime_sec)
    {
        Vector3 warpDir = new Vector3(0, 0, 1);
        _warpGrid.position += (_warpGrid.rotation * warpDir).ToUnityd() * primaryEngine.CalculateSpeedC() * Numbers.c * deltaTime_sec;
    }
    Quaternion CalculateTargetRotation(double currentTime_sec)
    {
        Quaternion lookRot = Quaternion.LookRotation(Vector3.forward, Vector3.up);

        Vector3 lookVector = -GetVectorTowardsTarget(currentTime_sec);
        Vector3 upVector = Vector3.up;

        Entity common = target.FindCommonAncestor(_warpGrid);
        Quaternion curRot_relCommon = _warpGrid.CalculateRotation_Relative(currentTime_sec, common);
        Quaternion curRot = _warpGrid.CalculateRotation(currentTime_sec);

        if (lookVector.magnitude > 0 && upVector.magnitude > 0)
        {
            lookRot = Quaternion.LookRotation(lookVector, upVector);
        }
        return curRot * lookRot;
    }

    public interface WarpState
    {
        public abstract void Update(Warp warp, float deltaTime_sec, double currentTime_sec);
    }
    public class WarpState_Init : WarpState
    {
        public void Update(Warp warp, float deltaTime_sec, double currentTime_sec)
        {
            if (warp.warpee.parent != null && warp.warpee.parent is OrbitalGrid)
            {
                warp._fromGrid = (OrbitalGrid)warp.warpee.parent;
            }
            else
            {
                warp._state = new WarpState_Exit();
                return;
            }

            Debug.Log("Initiate warp to target: " + warp.target.name);

            warp._warpGrid = new StationaryGrid()
            {
                name = "Warp Grid",
                position = warp.warpee.position,
                rotation = warp.warpee.rotation,
            };

            warp.warpee.position = new Vector3d(0, 0, 0);
            warp.warpee.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);

            warp.warpee.RemoveParent();
            warp._warpGrid.AddChild(warp.warpee);
            warp._fromGrid.AddChild(warp._warpGrid);

            warp._distance = 0;
            warp._positionOffset = warp._warpGrid.position;
            Debug.Log("WarpState_Init to WarpState_Align");
            warp._state = new WarpState_Align();
        }
    }
    public class WarpState_Align : WarpState
    {
        static double anglesPerSecond = 60;

        bool didInit = false;
        Quaternion startRotation;
        double startTime;
        double alignTime;
        public void Update(Warp warp, float deltaTime_sec, double currentTime_sec)
        {
            Quaternion currentRotation = warp._warpGrid.CalculateRotation(currentTime_sec);
            Quaternion targetRotation = warp.CalculateTargetRotation(currentTime_sec);

            float angleDiff = WarpManager.Instance.AngleBetweenVectors_Deg(currentRotation * Vector3.forward, targetRotation * Vector3.forward);
            if (angleDiff > 0.1f)
            {
                if (!didInit)
                {
                    didInit = true;
                    startRotation = currentRotation;
                    startTime = currentTime_sec;
                    alignTime = angleDiff / anglesPerSecond;
                }

                double elapsed = currentTime_sec - startTime;
                double percent = elapsed / alignTime;
                percent = System.Math.Min(percent, 1);
                warp._warpGrid.rotation = Quaternion.Slerp(startRotation, targetRotation, (float)percent);

                // Debug.LogFormat("Angle Diff: {0}, Align Percent: {1}", angleDiff, percent);
            }
            else
            {
                Debug.Log("WarpState_Align to WarpState_From");
                warp._state = new WarpState_From();
            }
        }
    }
    public class WarpState_From : WarpState
    {
        public void Update(Warp warp, float deltaTime_sec, double currentTime_sec)
        {
            Quaternion targetRotation = warp.CalculateTargetRotation(currentTime_sec);
            warp._warpGrid.rotation = targetRotation;

            warp._distance += warp.primaryEngine.CalculateSpeedC() * Numbers.c * deltaTime_sec;
            warp._warpGrid.position = warp._positionOffset + (warp._warpGrid.rotation * new Vector3(0, 0, (float)warp._distance)).ToUnityd();

            Vector3d posTo = warp._warpGrid.CalculatePosition_Relative(currentTime_sec, warp.target);

            if (warpThresh_m > posTo.magnitude && warp._distance > posTo.magnitude)
            {
                Quaternion rot = warp._warpGrid.CalculateRotation_Relative(currentTime_sec, warp.target);
                warp._warpGrid.rotation = rot;
                warp._warpGrid.position = posTo;

                warp._distance = posTo.magnitude;

                warp._warpGrid.RemoveParent();
                warp.target.AddChild(warp._warpGrid);

                Debug.Log("WarpState_From to WarpState_To");
                warp._state = new WarpState_To();
            }
            else if (warp._distance > warpThresh_m)
            {
                Entity common = warp._warpGrid.FindCommonAncestor(warp.target);
                
                Vector3d posCommon = warp._warpGrid.CalculatePosition_Relative(currentTime_sec, common);
                Quaternion rotCommon = warp._warpGrid.CalculateRotation_Relative(currentTime_sec, common);

                warp._warpGrid.position = posCommon;
                warp._warpGrid.rotation = rotCommon;
                warp._warpGrid.RemoveParent();
                common.AddChild(warp._warpGrid);

                Debug.Log("WarpState_From to WarpState_Transit");
                warp._state = new WarpState_Transit();
            }
        }
    }
    public class WarpState_Transit : WarpState
    {
        public void Update(Warp warp, float deltaTime_sec, double currentTime_sec)
        {
            warp._warpGrid.rotation = warp.CalculateTargetRotation(currentTime_sec);

            warp.UpdatePosition(deltaTime_sec);

            Vector3d posTo = warp._warpGrid.CalculatePosition_Relative(currentTime_sec, warp.target);

            if (warpThresh_m > posTo.magnitude)
            {
                Quaternion rot = warp._warpGrid.CalculateRotation_Relative(currentTime_sec, warp.target);
                warp._warpGrid.rotation = rot;
                warp._warpGrid.position = posTo;

                warp._distance = posTo.magnitude;

                warp._warpGrid.RemoveParent();
                warp.target.AddChild(warp._warpGrid);

                Debug.Log("WarpState_Transit to WarpState_To");
                warp._state = new WarpState_To();
            }
        }
    }
    public class WarpState_To : WarpState
    {
        public void Update(Warp warp, float deltaTime_sec, double currentTime_sec)
        {
            double endDistance_m = warp.target.radius_m;
            endDistance_m += System.Math.Max(5 * Numbers.KMToM, endDistance_m * 0.2);

            double remaining = (warp._distance - endDistance_m);

            double useSpeed = warp.primaryEngine.CalculateSpeedC() * Numbers.c;
            double maxSpeed;
            
            maxSpeed = (remaining * 2) + (warp.target.radius_m * 0.05);

            useSpeed = System.Math.Min(maxSpeed, useSpeed);

            warp._distance -= useSpeed * deltaTime_sec;

            warp._warpGrid.position = (warp._warpGrid.rotation * new Vector3(0, 0, -(float)warp._distance)).ToUnityd();



            if (endDistance_m >= warp._distance)
            {
                warp._distance = endDistance_m;
                warp._warpGrid.position = (warp._warpGrid.rotation * new Vector3(0, 0, -(float)warp._distance)).ToUnityd();

                Entity targetGrid;
                if (warp.target.parent is OrbitalGrid)
                {
                    targetGrid = warp.target.parent;

                    Vector3d posToTarget = warp._warpGrid.CalculatePosition(currentTime_sec);
                    Quaternion rotToGrid = warp._warpGrid.CalculateRotation_Relative(currentTime_sec, targetGrid);

                    Vector3 unit = Vector3.Normalize(posToTarget.ToUnity());
                    warp.warpee.position = (unit.ToUnityd() * endDistance_m) + warp.target.CalculatePosition(currentTime_sec);
                    warp.warpee.rotation = rotToGrid;
                }
                else
                {
                    Vector3d posToTarget = warp._warpGrid.CalculatePosition(currentTime_sec);
                    Quaternion rotToTarget = warp._warpGrid.CalculateRotation(currentTime_sec);
                    targetGrid = GridManager.Instance.CreateOrbitalGrid(warp.target, posToTarget);

                    warp.warpee.rotation = rotToTarget;
                }

                warp.warpee.RemoveParent();
                targetGrid.AddChild(warp.warpee);

                warp._warpGrid.RemoveParent();

                warp._isFinished = true;
                warp.primaryEngine.ResetSpeed();
                Debug.Log("WarpState_To to WarpState_Exit");
                warp._state = new WarpState_Exit();
            }
        }
    }
    public class WarpState_Exit : WarpState
    {
        public void Update(Warp warp, float deltaTime_sec, double currentTime_sec)
        {
            
        }
    }
}