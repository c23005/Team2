using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemaCameraScript : MonoBehaviour
{
    public bool IsInput;
    void Start()
    {
        Cinemachine.CinemachineCore.GetInputAxis = GetAxisCustom;
    }

    public float GetAxisCustom(string axisname)
    {
        if (!IsInput) return 0;
        if(axisname == "Mouse X")
        {
            if (Input.GetMouseButton(1))
            {
                return UnityEngine.Input.GetAxis("Mouse X");
            }
            else
            {
                return 0;
            }
        }
        if (axisname == "Mouse Y")
        {
            if (Input.GetMouseButton(1))
            {
                return UnityEngine.Input.GetAxis("Mouse Y");
            }
            else
            {
                return 0;
            }
        }

        return UnityEngine.Input.GetAxis(axisname);
    }
    void Update()
    {
        /*if (Input.GetMouseButton(1))
        {
            IsInput = true;
        }
        else
        {
            IsInput = false;
        }*/
    }
}
