using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    public static AudioManager Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }

            return new GameObject("SoundManager").AddComponent<AudioSource>().gameObject.AddComponent<AudioManager>();
        }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetSfxVolume(float volume)
    {
        PlayerPrefs.SetFloat("sfxVolume", volume);
        PlayerPrefs.Save();
    }

    public float GetSfxVolume()
    {
        return PlayerPrefs.GetFloat("sfxVolume", 1);
    }

    public static void PlaySound(string soundName)
    {
        AudioClip sound = Resources.Load<AudioClip>(Path.Combine("Sounds/", soundName));

        if (sound != null)
        {
            AudioSource.PlayClipAtPoint(sound, Vector3.zero, Instance.GetSfxVolume());
        }
        else
        {
            throw new System.Exception($"Sound '{soundName}' not found in the folder Sounds.");
        }
    }
}