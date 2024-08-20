using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameControllerscript : MonoBehaviour
{
    GameObject[] cats;
    GameObject[] movePoss;
    List<int> numbers = new List<int>();
    int startCatInt;
    List<int> CatInt = new List<int>();
    CatMoveScript[] script = new CatMoveScript[5];
    private void Awake()
    {
        cats = GameObject.FindGameObjectsWithTag("cat");
        movePoss = GameObject.FindGameObjectsWithTag("catMovePos");
        for(int i = 0; i < movePoss.Length; i++)
        {
            numbers.Add(i);
        }
        numbers = numbers.OrderBy(a => Guid.NewGuid()).ToList();
        for (int i = 0;i < cats.Length; i++)
        {
            CatInt.Add(i);
            script[i] = cats[i].GetComponent<CatMoveScript>();
            script[i].nextInt = numbers[i];
        }
    }
    void Start()
    {
        Application.targetFrameRate = 60;
    }


    void Update()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}
