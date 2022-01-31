using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShipController : MonoBehaviour
{
    public bool autoBrake;
    public Vector3 sizeMeters = new Vector3(20, 20, 20);

    SublightEngineMono sublightEngine;
    WarpEngineMono warpEngine;
    void Start()
    {
        sublightEngine = GetComponent<SublightEngineMono>();
        warpEngine = GetComponent<WarpEngineMono>();
        transform.localScale = sizeMeters / (float)Numbers.UnitsToMeters;
    }
    void Update()
    {
        /*
        if (enabled)
        {
            int sublightAcceleration = 0;
            Vector3 sublightRotation = new Vector3(0, 0, 0);

            if (warpEngine.warpFactor > 0 || Input.GetKey(KeyCode.LeftShift))
            {
                int warpAcceleration = 0;
                if (Input.GetKey(KeyCode.W))
                {
                    warpAcceleration = 1;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    warpAcceleration = -1;
                }

                warpEngine.moveDirection.z = warpAcceleration;
            }
            else
            {
                if (Input.GetKey(KeyCode.W))
                {
                    sublightAcceleration = 1;
                }
                else if (Input.GetKey(KeyCode.S) || autoBrake)
                {
                    sublightAcceleration = -1;
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

                sublightRotation.x = rotX;
                sublightRotation.y = rotY;
                sublightRotation.z = rotZ;
            }

            sublightEngine.moveDirection.z = sublightAcceleration;
            sublightEngine.rotateDirection = sublightRotation;
        }
        */

        int rotX = 0;
        int rotY = 0;
        int rotZ = 0;
        if (Input.GetKey(KeyCode.Q)) rotZ++;
        if (Input.GetKey(KeyCode.E)) rotZ--;
        if (Input.GetKey(KeyCode.A)) rotY--;
        if (Input.GetKey(KeyCode.D)) rotY++;
        if (Input.GetKey(KeyCode.Z)) rotX--;
        if (Input.GetKey(KeyCode.C)) rotX++;

        float rotSpeed = 0.2f;

        Quaternion applyRotation = Quaternion.Euler(rotSpeed * rotX, rotSpeed * rotY, rotSpeed * rotZ);
        PlayerManager.Instance.CharacterStructure.rotation *= applyRotation;
    }
}
