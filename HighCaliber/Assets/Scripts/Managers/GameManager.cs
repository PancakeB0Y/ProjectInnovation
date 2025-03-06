using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
