using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearSceneScript : MonoBehaviour
{
    public AudioSource AS;
    AudioClip clip;
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        clip = (AudioClip)Resources.Load("Sounds/SE/Click");
    }

    void Update()
    {
        
    }

    public void GameEnd(bool end)
    {
        //AS.Stop();
        //AS.volume = 1;
        AS.PlayOneShot(clip);
        if(end)
        {
            Invoke("end", 1);
        }
        else
        {
            Invoke("Retry", 1);
        }
    }

    void Retry()
    {
        SceneManager.LoadScene("GameScene");
    }

    void end()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

}
