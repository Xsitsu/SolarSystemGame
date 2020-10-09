using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShipController : MonoBehaviour
{
    public Vector3 sizeMeters = new Vector3(20, 20, 20);
    public bool autoBrake = false;
    [Range(0, 10000000)]
    public float maxSpeed = 100;
    [Range(0, 10000000)]
    public float acceleration = 42;
    [Range(0, 360 * 4)]
    public float rotateSpeed = 120;
    float speed;
    void Start()
    {
        speed = 0;
        transform.localScale = sizeMeters / (float)Numbers.UnitsToMeters;
    }
    void Update()
    {
        if (enabled)
        {
            if (Input.GetKey(KeyCode.W))
            {
                speed += acceleration * Time.deltaTime;
                if (speed > maxSpeed)
                {
                    speed = maxSpeed;
                }
            }
            else if (Input.GetKey(KeyCode.S) || autoBrake)
            {
                speed -= acceleration * Time.deltaTime;
                if (speed < 0)
                {
                    speed = 0;
                }
            }

            int rotX = 0;
            int rotY = 0;
            int rotZ = 0;
            if (Input.GetKey(KeyCode.Q)) rotZ++;
            if (Input.GetKey(KeyCode.E)) rotZ--;
            if (Input.GetKey(KeyCode.A)) rotY--;
            if (Input.GetKey(KeyCode.D)) rotY++;
            if (Input.GetKey(KeyCode.Z)) rotX--;
            if (Input.GetKey(KeyCode.C)) rotX++;

            Vector3 rotate = new Vector3(rotX, rotY, rotZ) * rotateSpeed * Time.deltaTime;
            transform.Rotate(rotate.x, rotate.y, rotate.z);
            transform.localPosition += transform.forward * (float)(speed / Numbers.UnitsToMeters) * Time.deltaTime;
        }
    }
}
