using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor.PackageManager;
using UnityEngine;

public class TouchControlsCylinder : MonoBehaviour
{
    [SerializeField] LayerMask bulletLayer;
    [SerializeField] LayerMask backgroundLayer;
    [SerializeField] LayerMask chamberLayer;

    BulletController selectedBullet;

    private void Start()
    {
        Screen.autorotateToPortrait = true;

        Screen.autorotateToPortraitUpsideDown = true;

        Screen.autorotateToLandscapeLeft = false;

        Screen.autorotateToLandscapeRight = false;

        Screen.orientation = ScreenOrientation.AutoRotation;
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
        selectedBullet = bulletController;
        selectedBullet.HoldBullet();
    }

    void DropBullet()
    {
        selectedBullet.DropBullet();
        selectedBullet = null;
    }

    bool PlaceBulletInChamber()
    {
        if (selectedBullet == null)
        {
            return false;
        }

        Collider[] hitColliders = Physics.OverlapSphere(selectedBullet.transform.position, 0.5f, chamberLayer);

        if (hitColliders.Length > 0)
        {
            selectedBullet.transform.position = hitColliders[0].transform.position;
            return true;
        }

        return false;
    }
}
