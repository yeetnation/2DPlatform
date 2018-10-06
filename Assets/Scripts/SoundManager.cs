//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public string mixerGroup;

    [Range(0f, 1f)]
    public float volume = 0.7f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1f;

    [Range(0f, 0.5f)]
    public float randomVolume = 0.1f;
    [Range(0f, 0.5f)]
    public float randomPitch = 0.1f;

    public bool loop = false;

    private AudioSource source;
    
    public void SetSource(AudioSource _source, AudioMixer _audioMixer)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
        source.outputAudioMixerGroup = _audioMixer.FindMatchingGroups(mixerGroup)[0];
    }

    public void Play()
    {
        source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
        source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }

    public bool IsPlaying()
    {
        return source.isPlaying;
    }

}

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;
    public AudioMixer audioMixer;

    [SerializeField]
    Sound[] sounds;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one SoundManager in the Scene.");
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        else
            instance = this;
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].name);
            _go.transform.SetParent(this.transform);
            sounds[i].SetSource(_go.AddComponent<AudioSource>(), audioMixer);
        }
        DontDestroyOnLoad(this);
    }
    

    public void PlaySound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Play();
                return;
            }
        }

        //no Sound with name
        Debug.LogWarning("SoundManager: Sounds not found in list: " + _name);
    }

    public void ChangeVolume(float _volume)
    {
        audioMixer.SetFloat("volume", 20f * Mathf.Log10(_volume));
    }

    public void ChangeMusic(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].mixerGroup == "Music" && sounds[i].IsPlaying())
            {
                sounds[i].Stop();
                PlaySound(_name);
                return;
            }
        }
        Debug.Log("No music was playing!");
        PlaySound(_name);
        return;
    }
}
