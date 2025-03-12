using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIDHolder : MonoBehaviour
{
    public string GUID => guid;

    [SerializeField]
    string guid;

    public void SetGUID(GUIDHolder newGUID)
    {
        guid = newGUID.GUID;
    }
}
