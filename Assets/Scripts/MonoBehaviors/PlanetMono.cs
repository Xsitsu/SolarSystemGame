using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMono : OrbitalBodyMono
{
    public PlanetTextureLookup lookup;
    public Shader planetShader;
    public Texture2D terrainTexture;
    public Texture2D lightTexture;
    Material material;
    GameObject _lightSource;

    Texture2D texture_white;
    Color color;

    void Start()
    {
        
    }
    void Awake()
    {
        material = new Material(planetShader);
        display.GetComponent<MeshRenderer>().material = material;
        texture_white = Texture2D.whiteTexture;
    }
    void Update()
    {
        if (_lightSource != null)
        {
            Vector3 dir = Vector3.Normalize(transform.position - _lightSource.transform.position);
            material.SetVector("_LightDirection", dir);
        }
    }
    public void InitTextures(Planet planet)
    {
        // Gross hacky function needs to be replaced with a proper solution sometime later
        if (planet.name == "Mercury")
        {
            terrainTexture = lookup.textures[1];
        }
        else if (planet.name == "Venus")
        {
            terrainTexture = lookup.textures[3];
        }
        else if (planet.name == "Earth")
        {
            terrainTexture = lookup.textures[4];
        }
        else if (planet.name == "Luna")
        {
            terrainTexture = lookup.textures[8];
        }
        else if (planet.name == "Mars")
        {
            terrainTexture = lookup.textures[9];
        }
        else if (planet.name == "Jupiter")
        {
            terrainTexture = lookup.textures[10];
        }
        else if (planet.name == "Saturn")
        {
            terrainTexture = lookup.textures[11];
        }
        else if (planet.name == "Uranus")
        {
            terrainTexture = lookup.textures[13];
        }
        else if (planet.name == "Neptune")
        {
            terrainTexture = lookup.textures[14];
        }
    }
    public void DisplayPlanet(Planet planet)
    {
        color = planet.color;

        display.transform.localScale *= (float)((2 * planet.radius_m) / Numbers.UnitsToMeters);

        material.SetColor("_Color", planet.color);
        material.SetTexture("_TerrainTexture", terrainTexture);
        material.SetTexture("_LightTexture", lightTexture);
    }
    public void SetLightSource(GameObject lightSource)
    {
        _lightSource = lightSource;
    }
    public void ApplySettings(StarSystemDisplaySettings settings)
    {
        material.SetFloat("_Ambience", settings.Ambience);
        if (settings.DebugTiling)
        {
            material.SetTexture("_TerrainTexture", terrainTexture);
         material.SetTexture("_LightTexture", lightTexture);
        }
        else
        {
            material.SetTexture("_TerrainTexture", texture_white);
            material.SetTexture("_LightTexture", texture_white);
        }
        if (settings.DebugAmbience)
        {
            // material.SetColor("_AmbientColor", settings.DebugAmbienceColor);
        }
        else
        {
            // material.SetColor("_AmbientColor", color);
        }
    }
}
