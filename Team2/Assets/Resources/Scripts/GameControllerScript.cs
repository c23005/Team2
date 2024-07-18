using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{
    public GameObject canvas;
    public GameObject eventsystem;
    private void Awake()
    {
        Instantiate(canvas,transform.position,Quaternion.identity);
        Instantiate(eventsystem,transform.position, Quaternion.identity);
    }
    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
