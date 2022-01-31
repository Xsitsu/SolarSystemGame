using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure
{
    public string name;
    public double radiusM;
    public Vector3d position = new Vector3d(0, 0, 0);
    public Quaternion rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);

    public OrbitalGrid parent { get; private set; }

    public void RemoveParent()
    {
        if (this.parent != null)
        {
            this.parent.RemoveStructure(this);
        }
    }
    public void AddTo(OrbitalGrid parent)
    {
        this.parent = parent;
    }
    public void RemoveFrom(OrbitalGrid parent)
    {
        if (this.parent == parent)
        {
            this.parent = null;
        }
    }
}
