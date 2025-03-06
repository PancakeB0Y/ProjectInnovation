using UnityEngine;

public class BulletController : MonoBehaviour
{
    Vector3 startPosition;

    Vector3 startRotation;
    Vector3 holdRotation;

    Vector3 startScale;
    Vector3 holdScale;

    BulletState state = BulletState.Default;

    void Start()
    {
        startPosition = transform.position;

        startRotation = new Vector3(-90, 0, 0);
        holdRotation = new Vector3(0, -90, 90);

        startScale = transform.localScale;
        holdScale = new Vector3(14.5f, 14.5f, 24.5f);
    }

    public void DropBullet()
    {
        state = BulletState.Default;

        transform.position = startPosition;
        transform.rotation = Quaternion.Euler(startRotation);

        transform.localScale = startScale;    
    }

    public void HoldBullet()
    {
        state = BulletState.Holding;

        transform.rotation = Quaternion.Euler(holdRotation);
        transform.localScale = holdScale;
    }

    public void LoadBullet()
    {
        state = BulletState.Loaded;
    }

    public bool IsLoaded()
    {
        if (state == BulletState.Loaded)
        {
            return true;
        }

        return false;
    }
}

public enum BulletState{ Default, Holding, Loaded }
