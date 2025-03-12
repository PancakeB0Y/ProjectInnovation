using System.Collections;
using UnityEngine;

public class CylinderTouchControls : MonoBehaviour
{
    [Header("Collision Layers")]
    [SerializeField] LayerMask bulletLayer;
    [SerializeField] LayerMask backgroundLayer;
    [SerializeField] LayerMask chamberLayer;

    [Header("Sounds")]
    public AudioClip cylinderRotationSound;
    public AudioClip bulletInSound;
    public AudioClip bulletOutSound;

    [Header("Cylinder Properties")]
    [SerializeField] int startingBulletCount = 0;
    [SerializeField] int chamberCount = 6;

    AudioSource audioSource;

    BulletController selectedBullet;

    private void OnEnable()
    {
        GyroManager.onSpin += ChangeScene;
    }

    private void OnDisable()
    {
        GyroManager.onSpin -= ChangeScene;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        GameManager.instance.loadedBulletsCount = startingBulletCount;
        GameManager.instance.chamberCount = chamberCount;
    }

    private void Update()
    {
        //Touch holding
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray cameraRay = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hitInfo;

                if (Physics.Raycast(cameraRay, out hitInfo, 100f, bulletLayer, QueryTriggerInteraction.Ignore))
                {
                    if (hitInfo.collider == null)
                    {
                        return;
                    }

                    GameObject bullet = hitInfo.collider.gameObject;

                    if (bullet == null)
                    {
                        return;
                    }

                    BulletController bulletController = bullet.GetComponent<BulletController>();

                    if (bulletController == null)
                    {
                        return;
                    }
                    SelectBullet(bulletController);
                }
            }
            else if (touch.phase == TouchPhase.Ended) {
                if (PlaceBulletInChamber())
                {
                    selectedBullet = null;
                }
                else
                {
                    DropBullet();
                }
                
            }
        }

        //Mouse holding
        if (Input.GetMouseButtonDown(0))
        {
            Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(cameraRay, out hitInfo, 100f, bulletLayer, QueryTriggerInteraction.Ignore))
            {
                if (hitInfo.collider == null)
                {
                    return;
                }

                GameObject bullet = hitInfo.collider.gameObject;

                if (bullet == null)
                {
                    return;
                }

                BulletController bulletController = bullet.GetComponent<BulletController>();

                if (bulletController == null)
                {
                    return;
                }
                SelectBullet(bulletController);
            }
        }

        if (selectedBullet != null) {
            Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            //Move bullet to mouse position
            if (Physics.Raycast(cameraRay, out hitInfo, 100f, backgroundLayer))
            {
                selectedBullet.transform.position = new Vector3(selectedBullet.transform.position.x, hitInfo.point.y, hitInfo.point.z - 0.3f);
            }
        }
    }

    void SelectBullet(BulletController bulletController)
    {
        if (bulletController == null)
        {
            return;
        }

        selectedBullet = bulletController;

        //Check if the selected bullet was loaded in a chamber
        if (selectedBullet.IsLoaded())
        {
            if (audioSource != null)
            {
                audioSource.PlayOneShot(bulletOutSound, 0.5f);

            }
            GameManager.instance.DecreaseBulletCount(1);
        }

        selectedBullet.HoldBullet();
    }

    void DropBullet()
    {
        if(selectedBullet == null)
        {
            return;
        }

        selectedBullet.DropBullet();
        selectedBullet = null;
    }

    bool PlaceBulletInChamber()
    {
        if (selectedBullet == null)
        {
            return false;
        }

        Collider[] hitColliders = Physics.OverlapSphere(selectedBullet.transform.position, 0.5f, chamberLayer, QueryTriggerInteraction.Ignore);

        if (hitColliders.Length > 0)
        {
            Debug.Log(hitColliders.Length);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                //Compare layerMask to layer
                if ((bulletLayer & (1 << hitColliders[i].gameObject.layer)) != 0)
                {
                    return false;
                }
            }

            selectedBullet.transform.position = hitColliders[0].transform.position;

            if (audioSource != null)
            {
                audioSource.PlayOneShot(bulletInSound, 0.5f);

            }
            //sets the bullet state
            selectedBullet.LoadBullet(transform);

            //increases the loaded bullet count
            GameManager.instance.IncreaseBulletCount(1);
            return true;
        }

        return false;
    }

    void ChangeScene()
    {
        if(GameManager.instance.loadedBulletsCount == 0)
        {
            return;
        }

        selectedBullet = null;
        StartCoroutine(RotateCylinder());
    }

    IEnumerator RotateCylinder()
    {
        if(audioSource != null)
        {
            audioSource.PlayOneShot(cylinderRotationSound, 0.5f);

        }

        StartCoroutine(RotateAnimation());

        yield return new WaitForSeconds(0.5f);
        
        GameManager.instance.GoToNextScene();
    }

    IEnumerator RotateAnimation()
    {
        while (true)
        {
            transform.Rotate(20, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
