using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMono : OrbitalBodyMono
{
    public Shader planetShader;
    Material material;
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
        
    }
    public void DisplayPlanet(Planet planet)
    {
        material.SetColor("_Color", planet.color);
        display.transform.localScale *= (float)((planet).radius / Numbers.UnitsToMeters);
    }
}
