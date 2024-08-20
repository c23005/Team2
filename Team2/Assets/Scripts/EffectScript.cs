using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectScript : MonoBehaviour
{
    public ParticleSystem[] effects = new ParticleSystem[3];
    float time;
    public float maxTime;
    public GameObject player;
    private void OnEnable()
    {
        for (int i = 0; i < effects.Length; i++)
        {
            effects[i].Play();
        }
        time = 0;
        transform.localPosition += new Vector3(0, 0.1f, 0);
        transform.parent = null;
    }

    void Start()
    {
        
    }

    void Update()
    {
        time += Time.deltaTime;
        if(time > maxTime)
        {
            transform.parent = player.transform;
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        transform.rotation = Quaternion.identity;
        transform.localPosition = new Vector3(0, 0, 0);
    }
}
