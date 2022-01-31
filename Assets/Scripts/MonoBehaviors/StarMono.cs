using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMono : OrbitalBodyMono
{
    public Shader starShader;
    public Texture2D texture;
    public GameObject lightSource;
    Material material;
    Light _light;
    void Start()
    {
        if (lightSource != null)
        {
            _light = lightSource.GetComponent<Light>();
        }
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
        material.SetTexture("_MainTex", texture);
        display.transform.localScale *= (float)((2 * star.radius_m) / Numbers.UnitsToMeters);

        if (_light != null)
        {
            _light.color = star.color;
        }
    }

    public void SetLightDirection(Quaternion dir)
    {
        if (_light != null)
        {
            _light.transform.localRotation = dir;
        }
    }
}
