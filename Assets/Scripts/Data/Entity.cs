using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity
{
    public string name;
    public double radius_m;
    public double mass_kg;


    public Entity parent;
    public List<Entity> children = new List<Entity>();

    public void SetEntityData(string name, double radius_m, double mass_kg)
    {
        this.name = name;
        this.radius_m = radius_m;
        this.mass_kg = mass_kg;
    }

    public void AddChild(Entity child)
    {
        if (!HasChild(child))
        {
            children.Add(child);
            child.AddTo(this);
        }
    }
    public void RemoveChild(Entity child)
    {
        if (HasChild(child))
        {
            child.RemoveFrom(this);
            children.Remove(child);
        }
    }
    public bool HasChild(Entity child)
    {
        foreach (Entity ch in children)
        {
            if (ch == child)
            {
                return true;
            }
        }
        return false;
    }
    public void RemoveParent()
    {
        if (this.parent != null)
        {
            this.parent.RemoveChild(this);
        }
    }
    void AddTo(Entity parent)
    {
        this.parent = parent;
    }
    void RemoveFrom(Entity parent)
    {
        if (this.parent == parent)
        {
            this.parent = null;
        }
    }

    public abstract Vector3d CalculatePosition(double atTime);
    public abstract Quaternion CalculateRotation(double atTime);
    public abstract Quaternion CalculateLocalRotation(double atTime);
}
