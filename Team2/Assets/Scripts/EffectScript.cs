using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectScript : MonoBehaviour
{
    public ParticleSystem[] effects = new ParticleSystem[3];
    float time;
    public float maxTime;
    public GameObject player;
    PlayerScript playerScript;
    AudioSource AS;
    private void OnEnable()
    {
        for (int i = 0; i < effects.Length; i++)
        {
            effects[i].Play();
        }
        time = 0;
        transform.localPosition += new Vector3(0, 0.1f, 0);
        transform.parent = null;
        if(gameObject.name == "LandingEffect2")
        {
            AS.Stop();
            AS.PlayOneShot(playerScript.AC[4]);
        }
    }

    void Start()
    {
        playerScript = player.GetComponent<PlayerScript>();
        AS = GetComponent<AudioSource>();
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
