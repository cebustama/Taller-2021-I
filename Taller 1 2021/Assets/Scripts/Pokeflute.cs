using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokeflute : MonoBehaviour
{
    public AudioClip clip;

    AudioSource s;

    // Start is called before the first frame update
    void Start()
    {
        s = gameObject.AddComponent<AudioSource>();
        s.clip = clip;
        s.volume = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            s.Play();

            GameObject snorlax = GameObject.Find("Snorlax");
            if (snorlax != null && 
            (snorlax.transform.position - transform.position).magnitude <= 5f)
            {
                snorlax.SetActive(false);
            }
        }
    }
}
