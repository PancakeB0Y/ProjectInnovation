using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gyro : MonoBehaviour
{
    void Start()
    {
        Input.gyro.enabled = true;
    }

    void Update()
    {
        Debug.Log(Input.gyro.attitude);
        transform.rotation = Input.gyro.attitude;
    }
}
