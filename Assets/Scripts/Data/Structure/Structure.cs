using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : Entity
{
    public Vector3d position = new Vector3d(0, 0, 0);
    public Quaternion rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);

    public override Vector3d CalculatePosition(double atTime)
    {
        return position;
    }
    public override Quaternion CalculateRotation(double atTime)
    {
        return rotation;
    }
}
