//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

[System.Serializable]
public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;
    public AudioMixer audioMixer;

    public float volMaster;
    public float volMusic;
    public float volSFX;

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

    public void PlaySoundOneShot(string _name)
    {
        instance.PlaySound(_name);
    }

    public void PlaySoundAtPoint(GameObject gObject, string _name)
    {
        gObject.AddComponent<AudioSource>();
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                gObject.GetComponent<AudioSource>().clip = sounds[i].clip;
                gObject.GetComponent<AudioSource>().spatialBlend = 1;
                gObject.GetComponent<AudioSource>().outputAudioMixerGroup = audioMixer.FindMatchingGroups(sounds[i].mixerGroup)[0];
                PlayClipAtPointCustom(gObject.GetComponent<AudioSource>(), gObject.transform.position);
                Destroy(gObject.GetComponent<AudioSource>());
                return;
            }
        }

        //no Sound with name
        Debug.LogWarning("SoundManager: Sounds not found in list: " + _name);
    }


    public void ChangeMasterVolume(float _volume)
    {
        volMaster = _volume;
        audioMixer.SetFloat("masterVol", 20f * Mathf.Log10(_volume));
    }

    public void ChangeMusicVolume(float _volume)
    {
        volMusic = _volume;
        audioMixer.SetFloat("musicVol", 20f * Mathf.Log10(_volume));
    }

    public void ChangeSfxVolume(float _volume)
    {
        volSFX = _volume;
        audioMixer.SetFloat("sfxVol", 20f * Mathf.Log10(_volume));
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

    public void SaveAudioSettings()
    {
        PlayerPrefs.SetFloat("pVolMaster", volMaster);
        PlayerPrefs.SetFloat("pVolMusic", volMusic);
        PlayerPrefs.SetFloat("pVolSFX", volSFX);

        Debug.Log("Saved Options");
    }

    public void LoadAudioSettings()
    {
        GameObject tempGO = new GameObject("TempSlider");

        if (PlayerPrefs.HasKey("pVolMaster") == false)
        {
            PlayerPrefs.SetFloat("pVolMaster", 1);
        }

        if (PlayerPrefs.HasKey("pVolMusic") == false)
        {
            PlayerPrefs.SetFloat("pVolMusic", 1);
        }

        if (PlayerPrefs.HasKey("pVolSFX") == false)
        {
            PlayerPrefs.SetFloat("pVolSFX", 1);
        }

        volMaster = PlayerPrefs.GetFloat("pVolMaster");
        volMusic = PlayerPrefs.GetFloat("pVolMusic");
        volSFX = PlayerPrefs.GetFloat("pVolSFX");

        if (GameObject.Find("SliderVolume") == false)
        {
            ChangeMasterVolume(volMaster);
            ChangeSfxVolume(volSFX);
            ChangeMusicVolume(volMusic);
            return;
        }

        GameObject.Find("SliderVolume").GetComponent<Slider>().value = volMaster;
        ChangeMasterVolume(volMaster);
        GameObject.Find("SliderSFX").GetComponent<Slider>().value = volSFX;
        ChangeSfxVolume(volSFX);
        GameObject.Find("SliderMusic").GetComponent<Slider>().value = volMusic;
        ChangeMusicVolume(volMusic);

    }

    private static AudioSource PlayClipAtPointCustom(AudioSource audioSource, Vector3 pos)
    {
        GameObject tempGO = new GameObject("TempAudio"); // create the temp object
        tempGO.transform.position = pos; // set its position
        AudioSource tempASource = tempGO.AddComponent<AudioSource>(); // add an audio source
        tempASource.clip = audioSource.clip;
        tempASource.outputAudioMixerGroup = audioSource.outputAudioMixerGroup;
        tempASource.mute = audioSource.mute;
        tempASource.bypassEffects = audioSource.bypassEffects;
        tempASource.bypassListenerEffects = audioSource.bypassListenerEffects;
        tempASource.bypassReverbZones = audioSource.bypassReverbZones;
        tempASource.playOnAwake = audioSource.playOnAwake;
        tempASource.loop = audioSource.loop;
        tempASource.priority = audioSource.priority;
        tempASource.volume = audioSource.volume;
        tempASource.pitch = audioSource.pitch;
        tempASource.panStereo = audioSource.panStereo;
        tempASource.spatialBlend = audioSource.spatialBlend;
        tempASource.reverbZoneMix = audioSource.reverbZoneMix;
        tempASource.dopplerLevel = audioSource.dopplerLevel;
        tempASource.rolloffMode = audioSource.rolloffMode;
        tempASource.minDistance = audioSource.minDistance;
        tempASource.spread = audioSource.spread;
        tempASource.maxDistance = audioSource.maxDistance;
        // set other aSource properties here, if desired
        tempASource.Play(); // start the sound
        MonoBehaviour.Destroy(tempGO, tempASource.clip.length); // destroy object after clip duration (this will not account for whether it is set to loop)
        return tempASource; // return the AudioSource reference
    }

}


