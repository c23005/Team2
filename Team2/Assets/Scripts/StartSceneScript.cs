using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneScript : MonoBehaviour
{
    void Start()
    {

    }


    void Update()
    {
        
    }

    public void OnStart()
    {
        SceneManager.LoadScene("GameScene");
    }


}
