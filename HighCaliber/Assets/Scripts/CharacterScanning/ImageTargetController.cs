using System;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ImageTargetController : MonoBehaviour
{
    public static event Action<string, string, string, string> OnTargetFound;
    public static event Action OnTargetLost;

    [Header("Data")]
    [SerializeField]
    string Id;
    [SerializeField]
    List<string> possibleAbilities;

    [Header("References")]
    [SerializeField]
    string cyllinderGUID;
    [SerializeField]
    string revolverGUID;

    public void TargetFound()
    {
        OnTargetFound?.Invoke(Id, possibleAbilities[UnityEngine.Random.Range(0, possibleAbilities.Count)],
            cyllinderGUID, revolverGUID);
    }

    public void TargetLost()
    {
        OnTargetLost?.Invoke();
    }
}
