using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneScript : MonoBehaviour
{
    public Text startString;
    float time;
    float speed;
    void Start()
    {

    }


    void Update()
    {
        startString.color = color(startString.color);
    }

    public void OnStart()
    {
        SceneManager.LoadScene("GameScene");
    }

    Color color(Color StrColor)
    {
        time += Time.deltaTime * 5;
        StrColor.a = Mathf.Sin(time);
        return StrColor;
    }

}
