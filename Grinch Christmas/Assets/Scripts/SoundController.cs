using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static AudioClip clickSound;
    public static AudioClip music;
    public static AudioSource audioSource;
    public static AudioSource musicSource;

    // Start is called before the first frame update
    void Start()
    {
        //load sounds
        clickSound = Resources.Load<AudioClip>("clickSound");
        music = Resources.Load<AudioClip>("music");

        //ref to audio src component
        audioSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void playMusic()
    {
        musicSource.clip = music;
        musicSource.Play();
        musicSource.volume = 0.05f;
        musicSource.loop = true;
    }

    public static void stopMusic() {
        musicSource.Stop();
    }

    public static void playSound(string name)
    {
        switch (name)
        {
            case "click":
                audioSource.PlayOneShot(clickSound);
                break;
        }
    }
}
