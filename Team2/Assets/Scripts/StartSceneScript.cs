using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneScript : MonoBehaviour
{
    AudioClip Click;
    public AudioSource AS;
    void Start()
    {
        Click = (AudioClip)Resources.Load("Sounds/SE/Click");
        AS = GetComponent<AudioSource>();
    }


    void Update()
    {
        
    }

    public void OnStart()
    {
        AS.PlayOneShot(Click);
        Invoke("Load", 1);
    }

    void Load()
    {
        SceneManager.LoadScene("GameScene");
    }

}
