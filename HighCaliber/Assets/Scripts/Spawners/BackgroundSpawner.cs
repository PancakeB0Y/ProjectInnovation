using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackgroundSpawner : MonoBehaviour
{
    [Header("Backgrounds")]
    [SerializeField]
    List<GUIDHolder> backgroundIds;

    void Start()
    {
        GUIDHolder background = backgroundIds.FirstOrDefault(b => b.GUID == CharacterSelectorManager.selectedCharacter.BackgroundId);
        if (background == null)
        {
            Debug.LogError("Wrong image target or background GUID!");
            return;
        }

        Instantiate(background.gameObject, background.transform.position, background.transform.rotation);
    }
}