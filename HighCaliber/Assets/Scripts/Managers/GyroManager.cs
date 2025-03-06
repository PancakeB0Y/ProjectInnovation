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
        if ((Input.deviceOrientation == DeviceOrientation.Portrait) && (previousDeviceOrientation == DeviceOrientation.LandscapeRight))
        {
            Debug.Log("BANG!!");
            onShoot?.Invoke();
        }

        //Track spinning motion
        if(Input.deviceOrientation == DeviceOrientation.FaceUp)
        {
            if(Input.gyro.rotationRateUnbiased.z > 3 || Input.gyro.rotationRateUnbiased.z < -3)
            {
                Debug.Log("SPIN");
                onSpin?.Invoke();
            }
        }

        previousDeviceOrientation = Input.deviceOrientation;
    }
}
