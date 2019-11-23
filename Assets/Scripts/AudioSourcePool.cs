using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePool : MonoBehaviour
{
    public int Amount = 10;
    public AudioSource AudioSourcePrefab;
    private List<AudioSource> _audios;

    private void Awake()
    {
        _audios = new List<AudioSource>();
        for (int i = 0; i < Amount; i++)
        {
            CreateAudioSource();
        }
    }

    private AudioSource CreateAudioSource()
    {
        AudioSource audio = GameObject.Instantiate(AudioSourcePrefab, this.transform);
        _audios.Add(audio);
        return audio;
    }

    public AudioSource GetSource()
    {
        foreach (AudioSource audio in _audios)
        {
            if (!audio.isPlaying)
            {
                return audio;
            }
        }
        return CreateAudioSource();

    }
}