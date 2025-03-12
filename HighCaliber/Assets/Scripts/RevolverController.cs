using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RevolverController : MonoBehaviour
{
    AudioSource audioSource;

    public AudioClip gunShotSound;
    public AudioClip gunMissSound;

    bool isShooting = false;

    System.Random r = new System.Random();

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        isShooting = true;
        StartCoroutine(InitialShootDelay());
    }

    private void OnEnable()
    {
        GyroManager.onShoot += Shoot;
    }

    private void OnDisable()
    {
        GyroManager.onShoot -= Shoot;
    }

    void Shoot()
    {
        if(audioSource == null)
        {
            return;
        }

        if (!isShooting)
        {
            StartCoroutine(ShootCoroutine());
        }
    }
    
    IEnumerator InitialShootDelay()
    {
        yield return new WaitForSeconds(0.25f);

        isShooting = false;
    }

    IEnumerator ShootCoroutine()
    {
        isShooting = true;
        if (CalculateShot())
        {
            audioSource.PlayOneShot(gunShotSound, 0.5f);
        }
        else
        {
            audioSource.PlayOneShot(gunMissSound, 1);
        }
        
        GameManager.instance.LoseAllBullets();

        yield return new WaitForSeconds(0.5f);

        isShooting = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    bool CalculateShot()
    {
        Debug.Log("bullets count: " + GameManager.instance.loadedBulletsCount + " Chamber count: " + GameManager.instance.chamberCount);

        int randomChamber = r.Next(GameManager.instance.chamberCount);

        if(randomChamber < GameManager.instance.loadedBulletsCount) {
            return true;
        }

        return false;
    }
}
