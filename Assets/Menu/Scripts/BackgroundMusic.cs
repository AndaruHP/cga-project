using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic Instance;

    [SerializeField] private AudioClip backgroundClip;
    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = backgroundClip;
            audioSource.loop = true;
            audioSource.playOnAwake = true;
            audioSource.volume = 0.5f; 

            audioSource.Play();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
