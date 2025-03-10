using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// DontDestroyOnLoad - Include in StartMenu scene<br></br><br></br>
/// 
/// Responsible for reading, writing and clearing data file
/// </summary>
public class FileReadWriteManager : MonoBehaviour
{
    public static FileReadWriteManager Instance => instance;
    static FileReadWriteManager instance;

    static string dataFilePath;

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

        CharacterSelectorManager.OnSelectedCharacter += WriteToFile;

        StartMenuUIManager.OnNewGameStarted += ClearFile;
    }

    void Start()
    {
        dataFilePath = Path.Combine(Application.persistentDataPath, "data.txt");
    }

    /// <summary>
    /// Read text content from data file
    /// </summary>
    /// <returns>If no file exists or file empty - return ""<br></br>
    /// else - return content</returns>
    public static string GetFileContent()
    {
        string existingContent = "";
        if (File.Exists(dataFilePath))
        {
            // Read the existing content
            existingContent = File.ReadAllText(dataFilePath);
        }

        return existingContent;
    }

    /// <summary>
    /// Write characted data to data file when character registered the first time
    /// </summary>
    /// <param name="data"></param>
    void WriteToFile(SelectedCharacterData data)
    {
        // Check in case game session is continued
        if (File.Exists(dataFilePath))
        {
            // Read the existing content
            string existingContent = File.ReadAllText(dataFilePath);

            // If the file is not empty, do nothing (or handle it differently)
            if (!string.IsNullOrWhiteSpace(existingContent))
            {
                Debug.Log("File already contains data");
                return;
            }
        }

        // File does not exist or is empty, so create/overwrite it
        using (StreamWriter sw = new StreamWriter(dataFilePath, false)) // 'false' overwrites
        {
            sw.WriteLine(data.Id);
            sw.WriteLine(data.ability);
        }
    }

    /// <summary>
    /// Clears file when new game is started
    /// </summary>
    void ClearFile()
    {
        // Creates the file if it does not exist and clears it
        using (StreamWriter sw = new StreamWriter(dataFilePath, false)) // false = overwrite mode
        {
            // Do nothing, just open and close it
        }
    }

    void OnDestroy()
    {
        CharacterSelectorManager.OnSelectedCharacter -= WriteToFile;

        StartMenuUIManager.OnNewGameStarted -= ClearFile;
    }
}