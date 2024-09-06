using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraScript : MonoBehaviour
{
    public GameObject player;
    PlayerScript playerScript;
    List<GameObject> cats = new List<GameObject>();
    void Start()
    {
        playerScript = player.GetComponent<PlayerScript>();
        cats = playerScript.catlist;
    }

    void Update()
    {
        for(int i = 0; i < cats.Count; i++)
        {
            var cameraPos = cats[i].transform.position;
        }


        transform.position = player.transform.position + new Vector3(0, 10, 0);
        transform.rotation = Quaternion.Euler(90, player.transform.eulerAngles.z, -player.transform.eulerAngles.y);
    }
}
