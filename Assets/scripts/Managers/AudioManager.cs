using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    public float realVolume;
    [SerializeField] float shopDistance;

    [Header("Lists")]
    readonly List<AudioSource> audioSourcePool = new();

    [Header("Transforms")]
    Transform shopkeeper;

    [Header("AudioSources")]
    [SerializeField] AudioSource mainMusicAudio;
    [SerializeField] AudioSource shopMusicAudio;

    [Header("Components")]
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioMixerGroup masterVolumeMixer;
    [SerializeField] AudioMixerGroup sfxVolumeMixer;
    [SerializeField] AudioMixerGroup musicVolumeMixer;

    #endregion

    #region StartUpdate

    void Start()
    {
        shopkeeper = GameObject.FindWithTag("Shop").transform.Find("orangutan");
    }

    void Update()
    {
        shopDistance = Vector2.Distance(transform.position, shopkeeper.position);

        if (shopDistance <= 20 && shopDistance > 15)
        {
            mainMusicAudio.volume = 0.2f - (0.2f / shopDistance);
            shopMusicAudio.volume = 0.2f / shopDistance;
        }
        if (shopDistance <= 15 && shopDistance > 10)
        {
            mainMusicAudio.volume = 0.2f - (1.2f / shopDistance);
            shopMusicAudio.volume = 1.2f / shopDistance;
        }
        if (shopDistance <= 10 && shopDistance >= 5)
        {
            mainMusicAudio.volume = 0.2f - (2f / shopDistance);
            shopMusicAudio.volume = 2f / shopDistance;

            if (shopMusicAudio.volume >= 0.2f)
            {
                mainMusicAudio.volume = 0f;
                shopMusicAudio.volume = 0.2f;
            }
        }
        if (shopDistance < 5)
        {
            mainMusicAudio.volume = 0f;
            shopMusicAudio.volume = 0.2f;
        }

        if (shopDistance > 20)
        {
            mainMusicAudio.volume = 0.2f;
            shopMusicAudio.volume = 0f;
        }
    }

    #endregion

    #region Methods

    AudioSource AddNewSourceToPool(string mixer)
    {
        if (mixer.ToLower() == "master")
        {
            audioMixer.GetFloat("masterVolume", out float dBMaster);
            float masterVolume = Mathf.Pow(10.0f, dBMaster / 20.0f);
            
            realVolume = masterVolume;
        }
        else if (mixer.ToLower() == "music")
        {
            audioMixer.GetFloat("musicVolume", out float dBMusic);
            float MusicVolume = Mathf.Pow(10.0f, dBMusic / 20.0f);

            audioMixer.GetFloat("masterVolume", out float dBMaster);
            float masterVolume = Mathf.Pow(10.0f, dBMaster / 20.0f);
            
            realVolume = (MusicVolume + masterVolume) / 2f;
        }
        else if (mixer.ToLower() == "sfx")
        {
            audioMixer.GetFloat("sfxVolume", out float dBSFX);
            float SFXVolume = Mathf.Pow(10.0f, dBSFX / 20.0f);

            audioMixer.GetFloat("masterVolume", out float dBMaster);
            float masterVolume = Mathf.Pow(10.0f, dBMaster / 20.0f);
            
            realVolume = (SFXVolume + masterVolume) / 2f;
        }
        
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.playOnAwake = false;
        newSource.volume = realVolume;
        newSource.spatialBlend = 1f;
        newSource.outputAudioMixerGroup = sfxVolumeMixer;
        audioSourcePool.Add(newSource);
        return newSource;
    }

    AudioSource GetAvailablePoolSource(string mixer)
    {
        //Fetch the first source in the pool that is not currently playing anything
        foreach (var source in audioSourcePool)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
 
        //No unused sources. Create and fetch a new source
        return AddNewSourceToPool(mixer);
    }

    public void PlayClip(AudioClip clip, string mixer)
    {
        AudioSource source = GetAvailablePoolSource(mixer);
        source.clip = clip;
        source.Play();
    }

    #endregion
}
