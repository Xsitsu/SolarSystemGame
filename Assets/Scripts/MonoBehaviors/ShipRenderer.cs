using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRenderer : MonoBehaviour
{
    MeshRenderer _renderer;
    Material _material;
    GameObject _lightSource;
    void Start()
    {
        _renderer = transform.GetComponent<MeshRenderer>();
        if (_renderer != null)
        {
            _material = _renderer.material;
        }
    }
    void Update()
    {
        if (_lightSource != null)
        {
            if (_material != null)
            {
                Vector3 dir = Vector3.Normalize(transform.position - _lightSource.transform.position);
                _material.SetVector("_LightDirection", dir);
            }
        }
    }

    public void SetLightSource(GameObject lightSource)
    {
        _lightSource = lightSource;
    }
}
