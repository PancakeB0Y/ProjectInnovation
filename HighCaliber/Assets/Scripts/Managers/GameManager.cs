using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public int loadedBulletsCount { get; private set; } = 0; 
    public int chamberCount { get; private set; } = 6;

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
}
