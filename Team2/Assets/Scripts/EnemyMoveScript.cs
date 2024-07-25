using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveScript : MonoBehaviour
{
    Transform[] movePoss;
    CapsuleCollider[] movePosCol = new CapsuleCollider[15];
    [Header("移動場所が子供にあるオブジェクト")]public GameObject movepos;
    public GameObject nextPosOBJ;
    Animator catanim;
    [HideInInspector]public Vector3 nextPos;
    float moveTime;
    int[] nowstatus = new int[3];
    int[] highstatus = new int[3];
    int[] samestatus = new int[4];
    int oldstatus;
    int startx = 3;
    int starty = 2;
    bool mainasu;
    bool stay = false;
    bool lookBool;
    bool nextBool;
    float stayTime = 0;
    int random;
    Quaternion Rote;
    Vector3 nowPos;
    int[,] nowpos = new int[5,7]
    {
        {9,9,9,9,9,9,9},
        {9,1,1,1,1,1,9},
        {9,1,1,1,1,1,9},
        {9,1,1,1,1,1,9},
        {9,9,9,9,9,9,9},
    };
    int[,] test = new int[5, 7]
    {
        {0,0,0,0,0,0,0},
        {0,1,2,3,2,1,0},
        {0,2,3,4,3,2,0},
        {0,1,2,3,2,1,0},    
        {0,0,0,0,0,0,0},
    };
    void Start()
    {
        catanim = GetComponent<Animator>();
        for (int i = 0; i < nowstatus.Length; i++)
        {
            nowstatus[i] = 0;
            highstatus[i] = 0;
        }
        movePoss = new Transform[movepos.transform.childCount];
        for(int i = 0; i < movepos.transform.childCount; i++)
        {
            movePoss[i] = movepos.transform.GetChild(i);
            movePosCol[i] = movePoss[i].gameObject.GetComponent<CapsuleCollider>();
        }
        moveCheak();
        Debug.Log(nextPos);
        mainasu = true;
        nextBool = true;
    }


    void Update()
    {
        nextPosOBJ.transform.position = nextPos;
        if(stay)
        {
            stayTime += Time.deltaTime;
            catanim.SetBool("Walk Bool", false);
            catanim.SetBool("Stay Bool", true);

        }
        //nextPosを見るように回転して、そのまま進む
        if (!stay)
        {
            nowPos = new Vector3(transform.position.x, 0, transform.position.z);
            if (nextBool)
            {
                nextBool = false;
            }
            Rote = Quaternion.LookRotation(nextPos - nowPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, Rote, Time.deltaTime *2);
            catanim.SetBool("Walk Bool", true);
        }
        //自分のxとzの座標がnestPosに着いたら発動
        if (Math.Round(transform.position.x,1) == Math.Round(nextPos.x, 1) && Math.Round(transform.position.z,1) == Math.Round(nextPos.z, 1))
        {
            random = UnityEngine.Random.Range(0, 10);
            nextBool = true;
            Debug.Log(random);
            if(random == 9)
            {
                stay = true;
            }
            else
            {
                moveCheak();
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
        int startInt = test[starty,startx];
        //startIntの数字が一番高い数字だったら低い数字のほうに行くようにしてstartIntの数字が一番低い数字だったら高い数字のほうに行くようにする
        if (startInt == 4)
        {
            mainasu = true;
            highstatus[2] = 4;
        }
        else if (startInt == 1)
        {
            mainasu = false;
            highstatus[2] = 1;
        }
        int cheakx = startx;
        int cheaky = starty;
        //Debug.Log("x : " + startx + "y ; " + starty);
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                if (x == 0 || y == 0)
                {
                    if(x != 0 ||  y != 0)
                    {
                        nowstatus[0] = cheakx + x;
                        nowstatus[1] = cheaky + y;
                        nowstatus[2] = test[cheaky + y, cheakx + x];
                        int cheakx2 = startx;
                        int cheaky2 = starty;
                        if (nowstatus[2] != 0)
                        {
                            if(mainasu)
                            {
                                //Debug.Log(nowstatus[2] + ":" + mainasu);
                                //Debug.Log(nowstatus[2] + ":" + highstatus[2]);
                                if (nowstatus[2] <= highstatus[2] && nowstatus[2] != 0)
                                {
                                    if (nowstatus[2] == highstatus[2])
                                    {
                                        int[] samex = new int[2];
                                        samex[0] = highstatus[0];
                                        samex[1] = cheakx + x;
                                        int[] samey = new int[2];
                                        samey[0] = highstatus[1];
                                        samey[1] = cheaky + y;
                                        int randomInt = 0;
                                        randomInt = UnityEngine.Random.Range(0, 2);
                                        highstatus[0] = samex[randomInt];
                                        highstatus[1] = samey[randomInt];
                                    }
                                    else
                                    {
                                        highstatus[0] = cheakx2 + x;
                                        highstatus[1] = cheaky2 + y;
                                    }
                                    highstatus[2] = nowstatus[2];
                                    //Debug.Log("マイナス");
                                }
                            }
                            else if(!mainasu)
                            {
                                if (nowstatus[2] >= highstatus[2] && nowstatus[2] != 0)
                                {
                                    if (nowstatus[2] == highstatus[2])
                                    {
                                        int[] samex = new int[2];
                                        samex[0] = highstatus[0];
                                        samex[1] = cheakx + x;
                                        int[] samey = new int[2];
                                        samey[0] = highstatus[1];
                                        samey[1] = cheaky + y;
                                        int randomInt = 0;
                                        randomInt = UnityEngine.Random.Range(0, 2);
                                        highstatus[0] = samex[randomInt];
                                        highstatus[1] = samey[randomInt];
                                    }
                                    else
                                    {
                                        highstatus[0] = cheakx2 + x;
                                        highstatus[1] = cheaky2 + y;
                                    }
                                    highstatus[2] = nowstatus[2];
                                    //Debug.Log("プラス");
                                }
                            }
                        }
                    }
                }
            }
        }
        nextPos = movePoss[5 * (highstatus[1] - 1) + (highstatus[0] - 1)].position;
        startx = highstatus[0]; starty = highstatus[1];
        //highstatus[2] = 0;
        oldstatus = highstatus[2];
    }

    private void OnTriggerEnter(Collider other)
    {
        //moveCheak();
    }

}
