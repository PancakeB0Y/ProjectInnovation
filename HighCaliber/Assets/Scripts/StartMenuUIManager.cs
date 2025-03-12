using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuUIManager : MonoBehaviour
{
    public static StartMenuUIManager Instance => instance;
    static StartMenuUIManager instance;

    public static event Action OnNewGameStarted;
    public static event Action<string> OnContinueGame;

    [SerializeField] GameObject RulesPanel;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            throw new InvalidOperationException("There can only be only one StartMenuUIManager in the scene!");
        }
    }

    public void StartNewGame()
    {
        OnNewGameStarted?.Invoke();
    }

    public void ContinueGame()
    {
        string fileContent = FileReadWriteManager.GetFileContent();

        if (fileContent == "")
        {
            StartNewGame();
        }
        else
        {
            OnContinueGame?.Invoke(fileContent);
        }
    }

    public void EnableRules()
    {
        RulesPanel.SetActive(true);
    }

    public void DisableRules()
    {
        RulesPanel.SetActive(false);
    }
}