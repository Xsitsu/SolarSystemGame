using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observable : MonoBehaviour
{
    [Range(0, 1000000000)]
    public float minZoom = 100;
    [Range(0, 1000000000)]
    public float maxZoom = 1000;
    [Range(0, 1000000000)]
    public float zoomSpeed = 100;
    [Range(0, 1000000000)]
    public float defaultZoom = 200;
    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
