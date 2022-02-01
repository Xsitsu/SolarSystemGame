using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMono : OrbitalBodyMono
{
    public Shader planetShader;
    public Texture2D texture;
    Material material;
    GameObject _lightSource;

    void Start()
    {
        
    }
    void Awake()
    {
        material = new Material(planetShader);
        display.GetComponent<MeshRenderer>().material = material;
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
        material.SetColor("_Color", planet.color);
        display.transform.localScale *= (float)((2 * planet.radius_m) / Numbers.UnitsToMeters);

        material.SetTexture("_MainTex", texture);
    }
    public void SetLightSource(GameObject lightSource)
    {
        _lightSource = lightSource;
    }
}
