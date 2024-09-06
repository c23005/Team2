using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItoScript : MonoBehaviour
{
    public float zScale;
    public float speed;
    float ypos;
    float y;
    public float maxSpeed;
    public bool onwool;
    Rigidbody rb;
    BoxCollider box;
    public Vector3 TouchPos;
    public GameObject touchPosOBJ;
    public GameObject player;
    Vector3 oldpos;
    Vector3 nowpos;
    PlayerScript playerScript;
    Vector3 StartPos;
    public bool outbool;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerScript = transform.parent.GetComponent<PlayerScript>();
        box = GetComponent<BoxCollider>();
        StartPos = transform.localPosition;
    }


    void Update()
    {
        nowpos = player.transform.position;
        Vector3 a = nowpos - oldpos;
        ypos = transform.position.y;
        if (!onwool)
        {
            if(zScale <= maxSpeed)zScale += 0.01f;
            transform.localScale = new Vector3(0.1f, 0.1f, transform.localScale.z + zScale);
            transform.Translate(0, 0, zScale / 2);
            if(!playerScript.AS.isPlaying)
            {
                playerScript.AS.Stop();
                playerScript.AS.PlayOneShot(playerScript.AC[1]);
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
            transform.LookAt(TouchPos);
            if (playerScript.z > 0)
            {
                box.isTrigger = true;
                //transform.position = Vector3.MoveTowards(transform.position,TouchPos, (maxSpeed / 2f) / 3);
                transform.position = transform.position - a;
                transform.Translate(0, 0, (maxSpeed / 2) / 1.5f);
                transform.localScale = new Vector3(0.1f, 0.1f, transform.localScale.z - (maxSpeed / 1.5f));
            }
            if(playerScript.x != 0)
            {
                box.isTrigger = true;
            }
        }
        oldpos = player.transform.position;
        if(transform.localScale.z > 50)
        {
            outbool = true;
        }
    }
    private void OnDisable()
    {
        box.isTrigger = false;
        zScale = 0;
        //糸を伸びる前まで戻す
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        transform.localPosition = StartPos;
        transform.localRotation = Quaternion.identity;
        //リギッドボディの勢いを無くす
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        onwool = false;
        player.transform.parent = null;
        //touchPosOBJの状態を元に戻す
        touchPosOBJ.transform.parent = player.transform;
        touchPosOBJ.transform.rotation = Quaternion.identity;
        touchPosOBJ.transform.localPosition = new Vector3(0, 0, 1.027f);
    }

    private void OnEnable()
    {
        //outbool = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Player" && !onwool)
        {
            playerScript.AS.Stop();
            TouchPos = collision.contacts[0].point;
            touchPosOBJ.transform.parent = null;
            touchPosOBJ.transform.position = TouchPos;
            touchPosOBJ.transform.rotation = Quaternion.identity;
            onwool = true;
        }
    }


}
