using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchColliderScript : MonoBehaviour
{
    CapsuleCollider CatchCol;
    GameObject catchOBJ;
    CatchScript catchScript;
    [Header("catだけを選択する")]public LayerMask catLayer;
    public bool isCatch;
    public GameObject cat;
    void Start()
    {
        catchOBJ = transform.GetChild(0).gameObject;
        catchScript = catchOBJ.GetComponent<CatchScript>();
        catchOBJ.SetActive(false);
        CatchCol = GetComponent<CapsuleCollider>();
        isCatch = false;
    }


    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            catchOBJ.SetActive(true);
        }
        if (Input.GetButton("Fire2"))
        {
            if (isCatch && catchScript.catchBool)
            {
                Destroy(cat);
                cat = null;
                isCatch = false;
                catchScript.catchBool = false;
            }
        }
        if (Input.GetButtonUp("Fire2"))
        {
            catchOBJ.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //中に入っているオブジェクトが猫かを調べる
        if (other.gameObject.tag == "cat")
        {
            isCatch = true;
            cat = other.gameObject;
            //other.gameObject.SetActive(false);
        }
        else
        {
            isCatch = false;
            cat = null;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "cat")
        {
            isCatch = false;
            other.gameObject.SetActive(true);
        }
    }

}
