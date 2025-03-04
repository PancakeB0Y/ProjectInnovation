using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor.PackageManager;
using UnityEngine;

public class TouchControls : MonoBehaviour
{
    [SerializeField] LayerMask bulletLayer;
    [SerializeField] LayerMask backgroundLayer;

    BulletController selectedBullet;

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
                DeselectBullet();
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

    void DeselectBullet()
    {
        selectedBullet.DropBullet();
        selectedBullet = null;
    }
}
