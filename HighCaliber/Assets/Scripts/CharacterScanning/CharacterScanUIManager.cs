using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterScanUIManager : MonoBehaviour
{
    public static CharacterScanUIManager Instance => instance;
    static CharacterScanUIManager instance;

    [SerializeField]
    RectTransform abilityTextCont;
    TextMeshProUGUI abilityTextField;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            throw new InvalidOperationException("There can only be only one CharacterScanUIManager in the scene!");
        }

        CharacterSelectorManager.OnTargetScanned += DisplayAbilityCont;
        CharacterSelectorManager.OnTargetLost += HideAbilityCont;
    }

    void Start()
    {
        abilityTextField = abilityTextCont.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    void DisplayAbilityCont(string abilityText)
    {
        if (abilityTextField.text == "") // Only the first time the chosen character is scanned
        {
            abilityTextField.text = abilityText;
        }

        abilityTextCont.gameObject.SetActive(true);
    }

    void HideAbilityCont()
    {
        abilityTextCont.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        CharacterSelectorManager.OnTargetScanned -= DisplayAbilityCont;
        CharacterSelectorManager.OnTargetLost -= HideAbilityCont;
    }
}
