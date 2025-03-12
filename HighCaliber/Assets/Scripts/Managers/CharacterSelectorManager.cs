using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Used as a container for the currently selected character
/// </summary>
public class SelectedCharacterData
{
    public string Id => id;
    public string Name => name;
    public string Ability => ability;
    public int AbilityIndex => abilityIndex;
    public string CylinderId => cylinderId;
    public string RevolverId => revolverId;
    public string BackgroundId => backgroundId;

    private string id;
    private string name;
    private int abilityIndex;
    private string ability;

    private string cylinderId;
    private string revolverId;
    private string backgroundId;

    public SelectedCharacterData(string id, string name, int abilityIndex, string ability, string cylinderId, string revolverId, string backgroundId)
    {
        this.id = id;
        this.name = name;
        this.abilityIndex = abilityIndex;
        this.ability = ability;

        this.cylinderId = cylinderId;
        this.revolverId = revolverId;
        this.backgroundId = backgroundId;
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
    /// When a character is registered - go to CylinderScene
    /// </summary>
    public static event Action OnCharacterRegistered;
    /// <summary>
    /// When an image target is found (scanned) - display character UI
    /// </summary>
    public static event Action<string, List<string>> OnTargetScanned;

    /// <summary>
    /// When an image target is lost - hide character UI
    /// </summary>
    public static event Action OnTargetLost;

    /// <summary>
    /// Holds the chosen character data throughout the whole game lifecycle
    /// </summary>
    public static SelectedCharacterData selectedCharacter = null;

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

        StartMenuUIManager.OnContinueGame += RegisterCharDataOnContinueGame;

        ImageTargetController.OnTargetFound += CheckForTargetFound;
        ImageTargetController.OnTargetLost += TargetLost;
    }

    /// <summary>
    /// Load characted data from data file
    /// </summary>
    /// <param name="charData">Text data from file</param>
    void RegisterCharDataOnContinueGame(string charData)
    {
        using (StringReader reader = new StringReader(charData))
        {
            string charGUID = reader.ReadLine();
            string charName = reader.ReadLine();
            int charAbilityIndex = int.Parse(reader.ReadLine());
            string charAbility = reader.ReadLine();
            string charCylinderId = reader.ReadLine();
            string charRevolverId = reader.ReadLine();
            string charBackgroundId = reader.ReadLine();

            selectedCharacter = new SelectedCharacterData(charGUID, charName, charAbilityIndex, charAbility, charCylinderId, charRevolverId, charBackgroundId);
        }

        OnCharacterRegistered?.Invoke();
    }

    /// <summary>
    /// Called when image target is found (scanned)
    /// </summary>
    public void CheckForTargetFound(string charId, string charName, int charAbilityIndex, string charAbility, List<string> charAbilities, string charCylinderId, string charRevolverId, string charBackgroundId)
    {
        if (selectedCharacter == null) // Assign a target
        {
            // Assign to selected character
            selectedCharacter = new SelectedCharacterData(charId, charName, charAbilityIndex, charAbility, charCylinderId, charRevolverId, charBackgroundId);

            // Write to a text file
            OnSelectedCharacter?.Invoke(selectedCharacter);
        }

        if (charId == selectedCharacter.Id) // Only show the ability
            OnTargetScanned?.Invoke(selectedCharacter.Ability, charAbilities);
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
        StartMenuUIManager.OnContinueGame -= RegisterCharDataOnContinueGame;

        ImageTargetController.OnTargetFound -= CheckForTargetFound;
        ImageTargetController.OnTargetLost -= TargetLost;
    }
}