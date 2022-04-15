using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCameraController : MonoBehaviour
{
    public GameObject target;
    public GameObject tweenContainer;
    public GameObject zoomContainer;

    [Range(0, 360*4)]
    public float rotationSpeed = 180;
    [Range(0, 1)]
    public float zoomStep = 0.2f;
    [Range(0, 100)]
    public float tweenTime = 2; // seconds
    float tweenSpeed = 0;

    float zoom = 400;
    float rotationHorizontal = 0;
    float rotationVertical = 0;
    Observable targ;

    void Start()
    {
        
    }

    void Update()
    {
        if (target != null)
        {
            Observe(target.GetComponent<Observable>());
            target = null;
        }

        if (Input.GetMouseButton(1))
        // if (UnityEngine.InputSystem.Mouse.current.rightButton.isPressed)
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

        zoom += Input.mouseScrollDelta.y * zoom * zoomStep;
        zoom = Mathf.Clamp(zoom, targ.minZoom, targ.maxZoom);

        float dist = tweenContainer.transform.localPosition.magnitude;
        if (dist > 0)
        {
            float travelDist = tweenSpeed * Time.deltaTime;
            if (dist > travelDist)
            {
                Vector3 dir = tweenContainer.transform.localPosition.normalized;
                tweenContainer.transform.localPosition -= dir * travelDist;
            }
            else
            {
                tweenContainer.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
    }

    void LateUpdate()
    {
        if (targ != null)
        {
            transform.localPosition = targ.transform.position;
            transform.eulerAngles = new Vector3(rotationVertical, rotationHorizontal, 0);
            zoomContainer.transform.localPosition = new Vector3(0, 0, -zoom / (float)Numbers.UnitsToMeters);
        }
    }

    public void Observe(Observable observable)
    {
        targ = observable;
        zoom = targ.defaultZoom;

        Vector3 diff = targ.transform.position - transform.position;
        transform.localPosition = targ.transform.position;
        tweenContainer.transform.localPosition = -diff;

        tweenSpeed = diff.magnitude / tweenTime;
    }
}
