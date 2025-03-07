using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UI;

public class TouchControlsCylinder : MonoBehaviour
{
    [SerializeField] LayerMask bulletLayer;
    [SerializeField] LayerMask backgroundLayer;
    [SerializeField] LayerMask chamberLayer;

    BulletController selectedBullet;

    private void OnEnable()
    {
        GyroManager.onSpin += ChangeScene;
    }

    private void OnDisable()
    {
        GyroManager.onSpin -= ChangeScene;
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
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if ((bulletLayer & (1 << hitColliders[i].gameObject.layer)) != 0)
                {
                    return false;
                }
            }


            selectedBullet.transform.position = hitColliders[0].transform.position;

            //sets the bullet state
            selectedBullet.LoadBullet();

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
        GameManager.instance.GoToNextScene();
    }
}
