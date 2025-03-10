using UnityEngine;

public class GyroManager : MonoBehaviour
{
    DeviceOrientation previousDeviceOrientation;

    public static System.Action onShoot;
    public static System.Action onSpin;

    void Start()
    {
        Input.gyro.enabled = true;

        previousDeviceOrientation = Input.deviceOrientation;
    }

    void Update()
    {
        //Track shooting motion
        if (Input.deviceOrientation == DeviceOrientation.LandscapeRight || Input.deviceOrientation == DeviceOrientation.LandscapeRight || Input.deviceOrientation == DeviceOrientation.Portrait || previousDeviceOrientation == DeviceOrientation.LandscapeRight || previousDeviceOrientation == DeviceOrientation.LandscapeRight)
        {
            if (Input.gyro.rotationRateUnbiased.z > 5 || Input.gyro.rotationRateUnbiased.z < -5)
            {
                Debug.Log("BANG!!");
                onShoot?.Invoke();
            }
        }

        //Track spinning motion
        if (Input.deviceOrientation == DeviceOrientation.FaceUp)
        {
            if(Input.gyro.rotationRateUnbiased.z > 4 || Input.gyro.rotationRateUnbiased.z < -4)
            {
                Debug.Log("SPIN");
                onSpin?.Invoke();
            }
        }

        previousDeviceOrientation = Input.deviceOrientation;
    }
}
