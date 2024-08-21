using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraScript : MonoBehaviour
{
    public GameObject player;
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = player.transform.position + new Vector3(0, 10, 0);
        transform.rotation = Quaternion.Euler(90, player.transform.eulerAngles.z, -player.transform.eulerAngles.y);
    }
}
