using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalGrid : Orbital
{
    public List<Structure> structures = new List<Structure>();

    public void AddStructure(Structure structure)
    {
        if (!HasStructure(structure))
        {
            structures.Add(structure);
            structure.AddTo(this);
        }
    }
    public void RemoveStructure(Structure structure)
    {
        if (HasStructure(structure))
        {
            structure.RemoveFrom(this);
            structures.Remove(structure);
        }
    }
    public bool HasStructure(Structure structure)
    {
        foreach (Structure s in structures)
        {
            if (s == structure)
            {
                return true;
            }
        }
        return false;
    }
}
