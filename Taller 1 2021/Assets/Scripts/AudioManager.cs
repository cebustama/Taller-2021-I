using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    private void Awake()
    {
        instance = this;

        if (musicSource == null)
            musicSource = gameObject.AddComponent<AudioSource>();
        else
            Debug.LogError("Falta asignar un AudioSource para la música en el AudioManager.");

        if (sfxSource == null) 
            sfxSource = gameObject.AddComponent<AudioSource>();
        else
            Debug.LogError("Falta asignar un AudioSource para los SFX en el AudioManager.");
    }

    public void ReproducirCancion(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void ReproducirEfecto(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
