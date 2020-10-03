using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMono : OrbitalBodyMono
{
    public Shader starShader;
    Material material;
    void Start()
    {
        
    }
    void Awake()
    {
        material = new Material(starShader);
        display.GetComponent<MeshRenderer>().material = material;
    }
    void Update()
    {
        
    }
    public void DisplayStar(Star star)
    {
        material.SetColor("_Color", star.color);
        display.transform.localScale *= (float)((2 * star.radius) / Numbers.UnitsToMeters);
    }
}
