using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject cat;
    Vector3 desiredPosition;
    float distance;
    Vector3 WallHitPos;
    Vector3 cameraPos;
    RaycastHit hit;
    bool shiftBool;
    public GameObject ground;
    public LayerMask layer;
    GameObject parent;
    void Start()
    {
        parent = gameObject.transform.parent.gameObject;
        distance = Vector3.Distance(cameraPos,cat.transform.position);
        desiredPosition = new Vector3(cat.transform.position.x, cat.transform.position.y + 0.5f, cat.transform.position.z - 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            shiftBool = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            shiftBool = false;
        }
        cameraPos = transform.position;
        parent.transform.position = cat.transform.position;
        float Mousex = Input.GetAxis("Mouse X");
        float Mousey = Input.GetAxis("Mouse Y");
        if (shiftBool)
        {
            if (Mathf.Abs(Mousex) >= 0.01f)
            {
                parent.transform.Rotate(0, Mousex, 0);
            }
            if (Mathf.Abs(Mousey) >= 0.01f)
            {
                parent.transform.Rotate(Mousey, 0, 0);
            }
        }
        /*if (WallCheck())
        {
            transform.position = WallHitPos;
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition,desiredPosition,1);
        }*/
    }

    bool WallCheck()
    {
        // Physics.Raycast(rayを出すスタート地点,rayの向き,当たったオブジェクトを取得,rayの発射距離,壁のレイヤー,)
        if(Physics.Raycast(cat.transform.position, cameraPos - cat.transform.position , out hit, Vector3.Distance(cat.transform.position, cameraPos), layer, QueryTriggerInteraction.Ignore))
        {
            WallHitPos = hit.point;
            //Debug.Log(WallHitPos);
            return true;
        }
        else
        {
            return false;
        }
    }

}
