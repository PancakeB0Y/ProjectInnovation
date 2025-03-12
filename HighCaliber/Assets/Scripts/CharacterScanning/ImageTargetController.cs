using System;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ImageTargetController : MonoBehaviour
{
    public static event Action<string, string, int, string, string, string> OnTargetFound;
    public static event Action OnTargetLost;

    [Header("Data")]
    [SerializeField]
    string Id;
    [SerializeField]
    string Name;
    [SerializeField]
    List<string> possibleAbilities;

    [Header("References")]
    [SerializeField]
    string cyllinderGUID;
    [SerializeField]
    string revolverGUID;

    public void TargetFound()
    {
        int abilityIndex = UnityEngine.Random.Range(0, possibleAbilities.Count);
        OnTargetFound?.Invoke(Id, Name, abilityIndex, possibleAbilities[abilityIndex],
            cyllinderGUID, revolverGUID);
    }

    public void TargetLost()
    {
        OnTargetLost?.Invoke();
    }
}
