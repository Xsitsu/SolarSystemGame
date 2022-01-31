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

    public bool HasAncestor(Entity entity)
    {
        Entity checkParent = this;
        while (checkParent != null && checkParent != entity)
        {
            checkParent = checkParent.parent;
        }

        if (checkParent == entity) return true;
        return false;
    }
    public bool HasDescendant(Entity entity)
    {
        return entity.HasAncestor(this);
    }
    Entity _CheckAncestry(Entity a, Entity b)
    {
        Entity checkParent = a;
        while (checkParent != null && !b.HasAncestor(checkParent))
        {
            checkParent = checkParent.parent;
        }
        return checkParent;
    }
    public Entity FindCommonAncestor(Entity entity)
    {
        Entity common = _CheckAncestry(entity, this);
        if (common == null)
        {
            common = _CheckAncestry(this, entity);
        }
        return common;
    }

    // My position relative to the other
    public Vector3d CalculatePosition_Relative(double atTime, Entity other)
    {
        if (other != this)
        {
            if (HasAncestor(other))
            {
                if (parent == other)
                {
                    return CalculatePosition(atTime);
                }
                else
                {
                    Quaternion parentRot = parent.CalculateRotation_Relative(atTime, other);
                    Vector3d localPosition = CalculatePosition(atTime);

                    Vector3d worldPosition = (parentRot * localPosition.ToUnity()).ToUnityd();
                    return parent.CalculatePosition_Relative(atTime, other) + worldPosition;
                }
            }
            else
            {
                Entity common = FindCommonAncestor(other);
                if (common != null)
                {
                    Vector3d commonToThis = CalculatePosition_Relative(atTime, common);
                    Vector3d commonToOther = other.CalculatePosition_Relative(atTime, common);

                    return commonToThis - commonToOther;
                }
            }
        }
        return new Vector3d();
    }

    public Quaternion CalculateRotation_Relative(double atTime, Entity other)
    {
        if (other != this)
        {
            if (HasAncestor(other))
            {
                if (parent == other)
                {
                    return CalculateRotation(atTime);
                }
                else
                {
                    return parent.CalculateRotation_Relative(atTime, other) * CalculateRotation(atTime);
                }
            }
            else
            {
                Entity common = FindCommonAncestor(other);
                if (common != null)
                {
                    Quaternion commonToThis = CalculateRotation_Relative(atTime, common);
                    Quaternion commonToOther = other.CalculateRotation_Relative(atTime, common);

                    return Quaternion.Inverse(commonToOther) * commonToThis;
                }
            }
        }
        return Quaternion.LookRotation(Vector3.forward, Vector3.up);
    }

    public abstract Vector3d CalculatePosition(double atTime);
    public abstract Quaternion CalculateRotation(double atTime);
    public abstract Quaternion CalculateLocalRotation(double atTime);
}
