using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] float musicVolume;

    [Range(0, 1)]
    [SerializeField] float sfxVolume;

    [SerializeField] AudioSource sfxAudioSource;
    [SerializeField] AudioSource musicAudioSource;


    private static AudioManager _instance;

    public static AudioManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AudioManager>();
                if (_instance == null)
                {
                    var prefab = Resources.Load("AudioBehaviour/AudioManager") as GameObject;
                    _instance = Instantiate(prefab, Vector3.zero, Quaternion.identity).GetComponent<AudioManager>();
                }
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        sfxAudioSource.volume = sfxVolume;
        musicAudioSource.volume = musicVolume;
    }

    public void PlayMusic(AudioClip audioClip)
    {
        musicAudioSource.clip = audioClip;
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }

    public void PlaySFX(AudioClip sfx)
    {
        sfxAudioSource.PlayOneShot(sfx);
    }
}
