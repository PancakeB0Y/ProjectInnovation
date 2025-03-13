using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterScanUIManager : MonoBehaviour
{
    public static CharacterScanUIManager Instance => instance;
    static CharacterScanUIManager instance;

    public static event Action OnGoToCylinderScene;

    [SerializeField]
    RectTransform abilityTextCont;
    TextMeshProUGUI abilityTextField;

    [Header("Anim fields")]
    [SerializeField]
    float startSpeed = 0.1f;
    [SerializeField]
    float endSpeed = 1.5f;
    [SerializeField]
    float duration = 2f;

    string chosenAbility;
    List<string> allAbilities = new List<string>();
    int counter = 0;

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

    public void GoToCylinderScene()
    {
        OnGoToCylinderScene?.Invoke();
    }

    void DisplayAbilityCont(string abilityText, List<string> abilitiesText)
    {
        if (abilityTextField.text == "") // Only the first time the chosen character is scanned
        {
            chosenAbility = abilityText;
            allAbilities = abilitiesText;

            // TODO: Play animation
            StartCoroutine(AbilityAnimCoroutine());
        }

        abilityTextCont.gameObject.SetActive(true);
    }

    IEnumerator AbilityAnimCoroutine()
    {
        float elapsedTime = 0f;
        float toggleSpeed = startSpeed;

        while (elapsedTime < duration)
        {
            // Toggle text
            abilityTextField.text = allAbilities[counter];

            counter++;
            if (counter >= allAbilities.Count)
                counter = 0;

            yield return new WaitForSeconds(toggleSpeed); // Wait before switching again

            // Gradually increase the delay (slowing down effect)
            toggleSpeed = Mathf.Lerp(startSpeed, endSpeed, elapsedTime / duration);
            elapsedTime += toggleSpeed;
        }

        // Ensure final state (chosen ability)
        abilityTextField.text = chosenAbility;
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