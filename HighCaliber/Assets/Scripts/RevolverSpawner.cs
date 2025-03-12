using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RevolverSpawner : MonoBehaviour
{
    [Header("Revolvers")]
    [SerializeField]
    List<GUIDHolder> revolverIds;

    void Start()
    {
        GUIDHolder revolver = null;
        if (CharacterSelectorManager.selectedCharacter.Name == "Lynx")
        {
            var revolvers = revolverIds.FindAll(r => r.GUID == CharacterSelectorManager.selectedCharacter.RevolverId);
            revolver = revolvers[CharacterSelectorManager.selectedCharacter.AbilityIndex];
        }
        else
        {
            revolver = revolverIds.FirstOrDefault(c => c.GUID == CharacterSelectorManager.selectedCharacter.RevolverId);
        }

        if (revolver == null)
        {
            Debug.LogError("Wrong image target or revolver GUID!");
            return;
        }

        Instantiate(revolver.gameObject, transform.position, Quaternion.identity);
    }
}
