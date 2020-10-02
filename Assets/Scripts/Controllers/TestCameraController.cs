using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCameraController : MonoBehaviour
{
    public GameObject target;
    public GameObject zoomContainer;
    [Range(0, 10000)]
    public float minZoom = 200;
    [Range(0, 10000)]
    public float maxZoom = 5000;
    [Range(0, 1000)]
    public float zoomSpeed = 400;
    [Range(0, 10000)]
    public float zoom = 400;
    [Range(0, 360*4)]
    public float rotationSpeed = 180;
    float rotationHorizontal = 0;
    float rotationVertical = 0;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            float axisX = Input.GetAxis("Mouse X");
            float axisY = Input.GetAxis("Mouse Y");
            if (axisX != 0)
            {
                rotationHorizontal += axisX * rotationSpeed;// * Time.deltaTime;
            }
            if (axisY != 0)
            {
                rotationVertical -= axisY * rotationSpeed;// * Time.deltaTime;
                rotationVertical = Mathf.Clamp(rotationVertical, -90, 90);
            }
        }

        zoom += Input.mouseScrollDelta.y * zoomSpeed;
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
    }

    void LateUpdate()
    {
        if (target != null)
        {
            transform.localPosition = target.transform.position;
            transform.eulerAngles = new Vector3(rotationVertical, rotationHorizontal, 0);
            zoomContainer.transform.localPosition = new Vector3(0, 0, -zoom / (float)Numbers.UnitsToMeters);
        }
    }
}
