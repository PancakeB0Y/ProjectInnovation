using System;
using System.IO;
using UnityEngine;

/// <summary>
/// Used as a container for the currently selected character
/// </summary>
public class SelectedCharacterData
{
    public string Id;
    public string ability;

    public SelectedCharacterData(string Id, string ability)
    {
        this.Id = Id;
        this.ability = ability;
    }
}

/// <summary>
/// DontDestroyOnLoad - Include in StartMenu scene<br></br><br></br>
/// 
/// Responsible for handling when image target is found and when lost<br></br>
/// Also, for registering the character data
/// </summary>
public class CharacterSelectorManager : MonoBehaviour
{
    public static CharacterSelectorManager Instance => instance;
    static CharacterSelectorManager instance;

    /// <summary>
    /// When a character is registered - write to the data file
    /// </summary>
    public static event Action<SelectedCharacterData> OnSelectedCharacter;
    /// <summary>
    /// When an image target is found (scanned) - display character UI
    /// </summary>
    public static event Action<string> OnTargetScanned;

    /// <summary>
    /// When an image target is lost - hide character UI
    /// </summary>
    public static event Action OnTargetLost;

    /// <summary>
    /// Holds the chosen character data throughout the whole game lifecycle
    /// </summary>
    SelectedCharacterData selectedCharacter = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return; // just to be sure
        }

        StartMenuUIManager.OnContinueGame += RegisterCharData;

        ImageTargetController.OnTargetFound += CheckForTargetFound;
        ImageTargetController.OnTargetLost += TargetLost;
    }

    /// <summary>
    /// Load characted data from data file
    /// </summary>
    /// <param name="charData">Text data from file</param>
    void RegisterCharData(string charData)
    {
        using (StringReader reader = new StringReader(charData))
        {
            string charGUID = reader.ReadLine();
            string charAbility = reader.ReadLine();

            selectedCharacter = new SelectedCharacterData(charGUID, charAbility);
        }
    }

    /// <summary>
    /// Called when image target is found (scanned)
    /// </summary>
    public void CheckForTargetFound(string charId, string charAbility)
    {
        if (selectedCharacter == null) // Assign a target
        {
            // Assign to selected character
            selectedCharacter = new SelectedCharacterData(charId, charAbility);

            // Write to a text file
            OnSelectedCharacter?.Invoke(selectedCharacter);
        }

        if (charId == selectedCharacter.Id) // Only show the ability
            OnTargetScanned?.Invoke(selectedCharacter.ability);
    }

    /// <summary>
    /// Called when image target is lost
    /// </summary>
    public void TargetLost()
    {
        OnTargetLost?.Invoke();
    }

    void OnDestroy()
    {
        StartMenuUIManager.OnContinueGame -= RegisterCharData;

        ImageTargetController.OnTargetFound -= CheckForTargetFound;
        ImageTargetController.OnTargetLost -= TargetLost;
    }
}