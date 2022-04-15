using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlanetTextureLookup", menuName = "ScriptableObjects/PlanetTextureLookup", order = 2)]
public class PlanetTextureLookup : ScriptableObject
{
    public List<Texture2D> textures;
}
