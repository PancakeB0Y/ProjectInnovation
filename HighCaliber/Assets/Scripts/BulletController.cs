using UnityEngine;

public class BulletController : MonoBehaviour
{
    Vector3 startPosition;

    Vector3 startRotation;
    Vector3 holdRotation;

    Vector3 startScale;
    Vector3 holdScale;

    BulletState state = BulletState.Default;

    Collider bulletCollider;

    Transform startParent;

    void Start()
    {
        startPosition = transform.position;

        startRotation = new Vector3(-90, 0, 0);
        holdRotation = new Vector3(0, -90, 90);

        startScale = transform.localScale;
        holdScale = new Vector3(20f, 20f, 20f);

        bulletCollider = GetComponent<Collider>();

        startParent = transform.parent;
    }

    public void DropBullet()
    {
        state = BulletState.Default;

        transform.SetParent(startParent);
        transform.position = startPosition;
        transform.rotation = Quaternion.Euler(startRotation);

        transform.localScale = startScale;


        if (bulletCollider != null)
        {
            bulletCollider.isTrigger = false;
        }
    }

    public void HoldBullet()
    {
        state = BulletState.Holding;

        transform.SetParent(startParent);
        transform.rotation = Quaternion.Euler(holdRotation);
        transform.localScale = holdScale;

        if (bulletCollider != null)
        {
            bulletCollider.isTrigger = true;
        }
    }

    public void LoadBullet(Transform parent)
    {
        state = BulletState.Loaded;
        transform.SetParent(parent);

        if (bulletCollider != null)
        {
            bulletCollider.isTrigger = false;
        }
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
