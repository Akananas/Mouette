using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    [SerializeField] Sound[] sounds;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.loop = sounds[i].loop;
            source.playOnAwake = false;
            source.volume = 1;
            source.pitch = 1;
            source.clip = sounds[i].clip;
            sounds[i].source = source;
        }
    }


    void PlayClip(string name)
    {
        Sound s = Array.Find<Sound>(sounds, s => s.name == name);
        s.source.Play();
    }
}



[System.Serializable]
public struct Sound
{
    public AudioClip clip;
    public bool loop;
    public string name;
    [HideInInspector] public AudioSource source;

    public Sound(AudioClip clip, AudioSource source, bool loop, string name)
    {
        this.clip = clip;
        this.source = source;
        this.loop = loop;
        this.name = name;
    }
}
