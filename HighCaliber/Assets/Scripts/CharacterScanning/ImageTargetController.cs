using System;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ImageTargetController : MonoBehaviour
{
    public static event Action<string, string> OnTargetFound;
    public static event Action OnTargetLost;

    [SerializeField]
    string Id;
    [SerializeField]
    List<string> possibleAbilities;

    public void TargetFound()
    {
        OnTargetFound?.Invoke(Id, possibleAbilities[UnityEngine.Random.Range(0, possibleAbilities.Count)]);
    }

    public void TargetLost()
    {
        OnTargetLost?.Invoke();
    }
}
