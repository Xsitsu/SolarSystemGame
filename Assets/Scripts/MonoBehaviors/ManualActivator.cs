using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public delegate void DoActivate();
public class ManualActivator : MonoBehaviour
{
    public bool Activate;
     public event DoActivate ActivateEvent;
    void Start()
    {

    }
    void Update()
    {
        if (Activate)
        {
            Activate = false;
            if (ActivateEvent != null)
            {
                ActivateEvent();
            }
        }
    }
}
