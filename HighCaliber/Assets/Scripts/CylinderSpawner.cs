using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CylinderSpawner : MonoBehaviour
{
    [Header("Cylinders")]
    [SerializeField]
    List<GUIDHolder> cylinderIds;

    void Start()
    {
        GUIDHolder cylinder = null;
        if (CharacterSelectorManager.selectedCharacter.Name == "Lynx")
        {
            var cylinders = cylinderIds.FindAll(c => c.GUID == CharacterSelectorManager.selectedCharacter.CylinderId);
            cylinder = cylinders[CharacterSelectorManager.selectedCharacter.AbilityIndex];
        }
        else
        {
            cylinder = cylinderIds.FirstOrDefault(c => c.GUID == CharacterSelectorManager.selectedCharacter.CylinderId);
        }

        if (cylinder == null)
        {
            Debug.LogError("Wrong image target or cylinder GUID!");
            return;
        }

        Instantiate(cylinder.gameObject, transform.position, Quaternion.identity);
    }
}