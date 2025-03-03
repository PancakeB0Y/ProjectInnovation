using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gyro : MonoBehaviour
{
    [SerializeField] float rotSpeed = 0.5f;
    Vector3 rot;

    DeviceOrientation previousDeviceOrientation;

    bool isShooting = false;
    bool isRotating = false;

    void Start()
    {
        rot = Vector3.zero;
        Input.gyro.enabled = true;

        previousDeviceOrientation = Input.deviceOrientation;
    }

    void Update()
    {
        rot.y = -Input.gyro.rotationRateUnbiased.z * rotSpeed;
        transform.Rotate(rot);   

        //Track shooting motion
        if (Input.deviceOrientation == DeviceOrientation.Portrait && (previousDeviceOrientation == DeviceOrientation.LandscapeLeft || previousDeviceOrientation == DeviceOrientation.LandscapeRight) && !isShooting)
        {
            Debug.Log("BANG!!");
            isShooting = true;
        }

        //Track spinning motion
        if(Input.deviceOrientation == DeviceOrientation.FaceUp && !isRotating)
        {
            if(Input.gyro.rotationRateUnbiased.z > 3 || Input.gyro.rotationRateUnbiased.z < -3)
            {
                Debug.Log("SPIN");
                isRotating = true;
            }
        }

        previousDeviceOrientation = Input.deviceOrientation;
    }
}
