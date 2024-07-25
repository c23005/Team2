using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatScript : MonoBehaviour
{
    public Animator catanim;
    public Animation catAnim;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        if(z == 1)
        {
            //transform.Translate(0, 0, 0.005f);
            catanim.SetBool("Walk Bool", true);
        }
        else
        {
            catanim.SetBool("Walk Bool", false);
        }
        catanim.SetInteger("Rote", (int)x);
    }
}
