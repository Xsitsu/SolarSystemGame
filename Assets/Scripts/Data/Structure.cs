using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : Orbital
{
    List<Module> _modules = new List<Module>();

    public void AddModule(Module module)
    {
        _modules.Add(module);
    }
    public void RemoveModule(Module module)
    {

    }
    public List<Module> GetModules()
    {
        return _modules;
    }
}
