using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public string typingAudioFolderPath;
    public string typingClipsPrefix = "Typing";
    public Sound[] sounds;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.audioMixerGroup;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Play("Theme");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning($"[AudioManager] Sound: {name} not found");
            return;
        }

        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null || s.source == null)
        {
            Debug.LogWarning($"[AudioManager] Sound or source for: {name} not found");
            return;
        }

        s.source.Stop();
    }

    public void PlayRandomTypingSound()
    {
        AudioClip[] loadedClips = Resources.LoadAll<AudioClip>(typingAudioFolderPath);
        
        if (loadedClips != null && loadedClips.Length > 0)
        {
            int randIdx = UnityEngine.Random.Range(0, loadedClips.Length);

            UpdateAudioClip(typingClipsPrefix, loadedClips[randIdx]);

            Play(typingClipsPrefix);
        }
    }

    private void UpdateAudioClip(string name, AudioClip clip)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s != null)
        {
            s.source.clip = clip;
        }
    }

    public bool IsSourcePlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s != null)
        {
            return s.source.isPlaying;
        }

        return false;
    }
}
