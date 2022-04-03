using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StarSystemDisplaySettings", menuName = "ScriptableObjects/StarSystemDisplaySettings", order = 1)]
public class StarSystemDisplaySettings : ScriptableObject
{
    [Range(0, 1)]
    public float Ambience = 0.01f;
    public bool DebugTiling = false;
    public bool DebugAmbience = false;
    public Color DebugAmbienceColor = new Color(1, 1, 1, 1);
}
