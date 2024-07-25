using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchColliderScript : MonoBehaviour
{
    CapsuleCollider CatchCol;
    [Header("cat������I������")]public LayerMask catLayer;
    public bool isCatch;
    public GameObject cat;
    void Start()
    {
        CatchCol = GetComponent<CapsuleCollider>();
        isCatch = false;
    }


    void Update()
    {
        Debug.Log("isCatch : " + isCatch);
        if (Input.GetButtonDown("Fire2"))
        {
            if(isCatch)
            {
                Destroy(cat);
                cat = null;
                isCatch = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //���ɓ����Ă���I�u�W�F�N�g���L���𒲂ׂ�
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
        if (other.gameObject.layer == catLayer)
        {
            isCatch = false;
            other.gameObject.SetActive(true);
        }
    }

}
