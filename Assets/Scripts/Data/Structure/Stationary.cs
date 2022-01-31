using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stationary : Entity
{
    public Vector3d position = new Vector3d(0, 0, 0);
    public Quaternion rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
    public Vector3d rotationSpeed_degPersec = new Vector3d(0, 0, 0);

    public override Vector3d CalculatePosition(double atTime)
    {
        return position;
    }
    public override Quaternion CalculateRotation(double atTime)
    {
        return rotation;
    }
    public override Quaternion CalculateLocalRotation(double atTime)
    {
        double x = (rotationSpeed_degPersec.x * atTime) % 360;
        double y = (rotationSpeed_degPersec.y * atTime) % 360;
        double z = (rotationSpeed_degPersec.z * atTime) % 360;

        return Quaternion.Euler((float)x, (float)y, (float)z);
    }
}
