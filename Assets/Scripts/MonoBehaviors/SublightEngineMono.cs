using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SublightEngineMono : EngineMono
{
    [Range(0, 299792458 * 0.1f)]
    public float maxSpeed = 100f; // m/s
    [Range(0, 299792458 * 0.1f)]
    public float acceleration = 42f; // m/s^2
    [Range(0, 360 * 4)]
    public float rotateSpeed = 120f; // degrees / s
    public float speed { get; private set; } // m/s

    public float SpeedOut;

    public Vector3 moveDirection;
    public Vector3 rotateDirection;

    void Start()
    {
        speed = 0;
        moveDirection = new Vector3(0, 0, 0);
        rotateDirection = new Vector3(0, 0, 0);
    }

    void Update()
    {
        speed += moveDirection.normalized.z * acceleration * Time.deltaTime;
        speed = Mathf.Clamp(speed, 0, maxSpeed);

        SpeedOut = speed;

        Vector3 rotateData = rotateDirection * rotateSpeed * Time.deltaTime;
        Quaternion rotationStep = Quaternion.Euler(rotateData.x, rotateData.y, rotateData.z);

        Vector3 moveStep = new Vector3(0, 0, (float)(speed) * Time.deltaTime);

        Structure characterStructure = PlayerManager.Instance.CharacterStructure;
        characterStructure.rotation *= rotationStep;
        characterStructure.position += (characterStructure.rotation * moveStep).ToUnityd();
    }
}
