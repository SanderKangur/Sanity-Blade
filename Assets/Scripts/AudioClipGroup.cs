using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AudioClipGroup")]
public class AudioClipGroup : ScriptableObject
{
    [Range(0, 2)]
    public float VolumeMin = 1;
    [Range(0, 2)]
    public float VolumeMax = 1;
    [Range(0, 2)]
    public float PitchMin = 1;
    [Range(0, 2)]
    public float PitchMax = 1;
    public float Cooldown = 0.1f;
    public List<AudioClip> AudioClips;

    private float _timestamp;
    private AudioSourcePool _pool;

    public void Play(AudioSource audio)
    {
        if (Time.time < _timestamp) return;

        if (AudioClips.Count == 0 && AudioClips == null) return;

        audio.clip = AudioClips[Random.Range(0, AudioClips.Count)];
        audio.volume = Random.Range(VolumeMin, VolumeMax);
        audio.pitch = Random.Range(PitchMin, PitchMax);
        audio.Play();
        _timestamp = Time.time + Cooldown;

    }

    public void OnEnable()
    {
        _timestamp = 0;
    }

    public void Play()
    {
        if (_pool == null)
        {
            _pool = FindObjectOfType<AudioSourcePool>();
        }
        Play(_pool.GetSource());
    }
}