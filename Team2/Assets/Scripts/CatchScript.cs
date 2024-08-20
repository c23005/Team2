using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchScript : MonoBehaviour
{
    CatchColliderScript catchScr;
    float time;
    [HideInInspector]public bool catchBool;
    void Start()
    {
        catchScr = transform.parent.GetComponent<CatchColliderScript>();
    }


    void Update()
    {
        time += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "cat")
        {
            if (time < 0.3f)
            {
                catchBool = true;
            }
        }
    }
    private void OnDisable()
    {
        time = 0;
        catchBool = false;
    }
}
