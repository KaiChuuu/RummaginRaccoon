using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string Name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 0.7f;

    private float pitch = 1f;

    [Header("Variance")]
    [Range(0f, 0.5f)]
    public float randomVolume = 0.1f;

    [Range(0f, 0.5f)]
    public float randomPitch = 0.1f;

    private AudioSource source;

    [Space(10f)]
    public bool loops = false;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
    }

    public void ChangeVolume(float vol)
    {
        if (source != null)
            source.volume = vol * volume;
    }

    public void Play()
    {
        if (clip == null)
            return;

        source.volume = volume * (1 + UnityEngine.Random.Range(-randomVolume / 2f, randomVolume / 2f));
        source.pitch = pitch * (1 + UnityEngine.Random.Range(-randomPitch / 2f, randomPitch / 2f));
        source.loop = loops;

        source.Play();
    }

    public void Stop()
    {
        if (source.isPlaying)
        {
            source.Stop();
        }
    }
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    private List<Sound> sounds = new();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        foreach (Sound sound in sounds)
        {
            GameObject _go = new GameObject(sound.Name);
            _go.transform.SetParent(transform, false);
            sound.SetSource(_go.AddComponent<AudioSource>());
        }
    }

    private void Start()
    {
        Unmute();
    }

    public void Mute()
    {
        for (int i = 0; i < sounds.Count; i++)
        {
            sounds[i].Stop();
        }
    }

    public void Unmute()
    {
        PlaySound("BGM");
    }

    public void PlaySound(string _name)
    {
        for (int i = 0; i < sounds.Count; i++)
        {
            if (sounds[i].Name == _name)
            {
                sounds[i].Play();
                return;
            }
        }

        Debug.LogWarning(_name + " not found in Audio Manager");
    }
}
