using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMono : OrbitalBodyMono
{
    public Shader planetShader;
    public Texture2D texture;
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
    public void DisplayPlanet(Planet planet)
    {
        color = planet.color;

        display.transform.localScale *= (float)((2 * planet.radius_m) / Numbers.UnitsToMeters);

        material.SetColor("_Color", planet.color);
        material.SetTexture("_MainTex", texture);
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
            material.SetTexture("_MainTex", texture);
        }
        else
        {
            material.SetTexture("_MainTex", texture_white);
        }
        if (settings.DebugAmbience)
        {
            material.SetColor("_AmbientColor", settings.DebugAmbienceColor);
        }
        else
        {
            material.SetColor("_AmbientColor", color);
        }
    }
}
