using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMoveScript : MonoBehaviour
{
    public GameObject[] MoveOBJ;
    int oldMovePos;
    int moveInt;
    Transform[] movePos;
    Animator catanim;
    Vector3 nowPos;
    Quaternion Rote;
    bool mainasu;
    bool stay = false;
    float stayTime;
    void Start()
    {
        MovePosPick();
        int startPos = UnityEngine.Random.Range(0, movePos.Length);
        transform.position = movePos[startPos].position;
        moveInt = startPos;
        if(moveInt == movePos.Length - 1)
        {
            mainasu = true;
        }
        Debug.Log(moveInt);
        //moveInt = 0;
        catanim = GetComponent<Animator>();
    }


    void Update()
    {
        Vector3 nextPos = movePos[moveInt].position;
        if (stay)
        {
            stayTime += Time.deltaTime;
            catanim.SetBool("Walk Bool", false);
            catanim.SetBool("Stay Bool", true);

        }
        //Debug.Log(nextPos);
        if (!stay)
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
            if(random == 0)
            {
                stay = true;
            }
            if(random % 3 == 0)
            {
                catanim.SetBool("DashBool", true);
                catanim.SetBool("Walk Bool", false);
            }
            else
            {
                catanim.SetBool("DashBool", false);
                catanim.SetBool("Walk Bool", true);
            }
        }
        if (stayTime >= 10)
        {
            stay = false;
            stayTime = 0;
            //moveCheak();
            catanim.SetBool("Stay Bool", false);
            //catanim.SetBool("Walk Bool", true);
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
        int nextInt = UnityEngine.Random.Range(0, MoveOBJ.Length);
        Debug.Log("NextInt : " + nextInt);
        for(int i = 0; i < MoveOBJ[nextInt].transform.childCount; i++)
        {
            Array.Resize(ref movePos, i + 1);
            movePos[i] = MoveOBJ[nextInt].transform.GetChild(i).transform;
        }
    }

}
