using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            sounds[i].source = source;
        }
    }
}

[System.Serializable]
public struct Sound
{
    public AudioClip clip;
    public bool loop;
    [HideInInspector] public AudioSource source;

    public Sound(AudioClip clip, AudioSource source, bool loop)
    {
        this.clip = clip;
        this.source = source;
        this.loop = loop;
    }
}
