using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class MinimapCameraScript : MonoBehaviour
{
    public GameObject player;
    PlayerScript playerScript;
    public List<GameObject> cat = new List<GameObject>();
    public List<GameObject> mini = new List<GameObject>();
    public Material[] materials = new Material[2];
    void Start()
    {
        playerScript = player.GetComponent<PlayerScript>();
        for(int i = 0;i < cat.Count; i++)
        {
            //mini[i] = cat[i].gameObject.transform.GetChild(2).gameObject;
        }
    }

    void Update()
    {
        transform.position = player.transform.position + new Vector3(0, 10, 0);
        transform.rotation = Quaternion.Euler(90, player.transform.eulerAngles.z, -player.transform.eulerAngles.y);
        var miniPos = new Vector3(transform.position.x, 0, transform.position.z);
        cat.RemoveAll (item => item == null);
        mini.RemoveAll(minimap => minimap == null);
        for (int i = 0;i < cat.Count;i++)
        {
            var catLenght = Vector3.ClampMagnitude(cat[i].transform.position, 6);
            var catPos = new Vector3(cat[i].transform.position.x, player.transform.position.y, cat[i].transform.position.z);
            float catDir = Vector3.Distance(player.transform.position, catPos);
            if (catDir > 7 || catDir < -7)
            {
                mini[i].GetComponent<MeshRenderer>().material = materials[0];
                var offset = catPos - player.transform.position;
                mini[i].transform.position = player.transform.position + Vector3.ClampMagnitude(offset, 6);
            }
            else
            {
                mini[i].transform.position = cat[i].transform.position;
                mini[i].GetComponent<MeshRenderer>().material = materials[1];
            }
        }


    }
}
