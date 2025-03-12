using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public int loadedBulletsCount = 0; 
    public int chamberCount = 6;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        StartMenuUIManager.OnNewGameStarted += GoToNextScene;

        CharacterScanUIManager.OnGoToCylinderScene += GoToCylinderScene;
        CharacterSelectorManager.OnCharacterRegistered += GoToCylinderScene;
    }

    public void IncreaseBulletCount(int bulletCount)
    {
        loadedBulletsCount += bulletCount;
    }

    public void DecreaseBulletCount(int bulletCount)
    {
        loadedBulletsCount -= bulletCount;
    }

    public void LoseAllBullets()
    {
        loadedBulletsCount = 0;
    }

    public void GoToCylinderScene()
    {
        GoToScene("CylinderScene");
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void GoToNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GoToPreviousScene()
    {
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        else
        {
            Debug.Log("You are in the first scene");
        }
    }

    public void GoToHomeScene()
    {
        SceneManager.LoadScene(0);
    }

    void OnDestroy()
    {
        StartMenuUIManager.OnNewGameStarted -= GoToNextScene;

        CharacterScanUIManager.OnGoToCylinderScene += GoToCylinderScene;
    }
}
