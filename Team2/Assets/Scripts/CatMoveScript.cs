using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMoveScript : MonoBehaviour
{
    public GameObject[] MoveOBJ;
    int oldMovePos;
    public int moveInt;
    Transform[] movePos;
    Animator catanim;
    Vector3 nowPos;
    Quaternion Rote;
    public bool mainasu;
    bool stay = false;
    float stayTime;
    RaycastHit hit;
    LayerMask catlayer;
    bool isForward = false;
    bool isside;
    bool startBool = true;
    Quaternion kaihiRote;
    [HideInInspector]public int nextInt;
    bool catforward()
    {
        if(Physics.Raycast(transform.position,transform.forward,out hit, 2, catlayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool catside()
    {
        if (Physics.Raycast(transform.position, transform.right, out hit, 2, catlayer))
        {
            return true;
        }
        else if(Physics.Raycast(transform.position, -transform.right, out hit, 2, catlayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Start()
    {
        catlayer = 1 << gameObject.layer;
        MovePosPick();
        startBool = false;
        int startPos = UnityEngine.Random.Range(1, movePos.Length);
        transform.position = movePos[startPos].position;
        moveInt = startPos;
        if(moveInt == movePos.Length - 1)
        {
            mainasu = true;
        }
        //Debug.Log(gameObject.name + "�̈ʒu : " + nextInt + ":" + moveInt);
        //Debug.Log(moveInt);
        //moveInt = 0;
        catanim = GetComponent<Animator>();
    }


    void Update()
    {
        catforward();
        catside();
        Vector3 nextPos = movePos[moveInt].position;
        if (catforward())
        {
            isForward = true;
        }
        if (catside())
        {
            isForward = false;
        }
        if(isForward)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,kaihiRote * Quaternion.Euler(0,20,0),Time.deltaTime * 2);
        }
        else
        {
            kaihiRote = transform.rotation;
        }
        if (stay)
        {
            stayTime += Time.deltaTime;
            catanim.SetBool("Walk Bool", false);
            catanim.SetBool("Stay Bool", true);

        }
        //Debug.Log(nextPos);
        if (!stay && !isForward)
        {
            nowPos = new Vector3(transform.position.x, 0, transform.position.z);
            Rote = Quaternion.LookRotation(nextPos - nowPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, Rote, Time.deltaTime * 2);
            
        }
        if (Math.Round(transform.position.x, 1) == Math.Round(nextPos.x, 1) && Math.Round(transform.position.z, 1) == Math.Round(nextPos.z, 1))
        {
            //Debug.Log("moveInt : " + moveInt);
            moveCheak();
            int random = UnityEngine.Random.Range(0, 10);
            //Debug.Log("random : " + random);
            /*if (random == 0)
            {
                stay = true;
            }*/
                catanim.SetBool("DashBool", false);
                catanim.SetBool("Walk Bool", true);
        }
        if (stayTime >= 10)
        {
            stay = false;
            stayTime = 0;
            //moveCheak();
            catanim.SetBool("Stay Bool", false);
            catanim.SetBool("Walk Bool", true);
        }
    }

    void moveCheak()
    {
        if (!mainasu) moveInt++;
        else moveInt--;
        if (moveInt == movePos.Length - 1)
        {
            mainasu = true;
        }
        if (mainasu && moveInt == 0)
        {
            mainasu = false;
            Array.Clear(movePos, 0, movePos.Length);
            MovePosPick();
        }
    }

    void MovePosPick()
    {
        if (!startBool)
        {
            nextInt = UnityEngine.Random.Range(0, MoveOBJ.Length);
        }
        //Debug.Log("NextInt : " + nextInt);
        for(int i = 0; i < MoveOBJ[nextInt].transform.childCount; i++)
        {
            Array.Resize(ref movePos, i + 1);
            movePos[i] = MoveOBJ[nextInt].transform.GetChild(i).transform;
        }
    }

}
