using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum VolumeType
{
    MUSIC,
    SOUND
}

public class VolumeManager : MonoBehaviour
{
    private List<GameObject> musicObjects;
    private List<GameObject> sfxObjects;
    
    void Start()
    {
        musicObjects = GameObject.FindGameObjectsWithTag("SFX_Music").ToList();
        sfxObjects = GameObject.FindGameObjectsWithTag("SFX_Sound").ToList();
        
        SetVolume(VolumeType.MUSIC, PlayerPrefs.GetFloat("MusicVolume"));
        SetVolume(VolumeType.SOUND, PlayerPrefs.GetFloat("SoundVolume"));
    }

    public void SetVolume(VolumeType volume,  float value)
    {
        if (volume == VolumeType.MUSIC)
        {
            foreach (GameObject obj in musicObjects)
            {
                AudioSource source = obj.GetComponent<AudioSource>();
                source.volume = value;
            }
        } else if (volume == VolumeType.SOUND)
        {
            foreach (GameObject obj in sfxObjects)
            {
                AudioSource source = obj.GetComponent<AudioSource>();
                source.volume = value;
            }
        }
    }
}
