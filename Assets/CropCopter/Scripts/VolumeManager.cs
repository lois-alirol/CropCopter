using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * Les différents type de volume changeables (musique et effets)
 */
public enum VolumeType
{
    MUSIC,
    SOUND
}

public class VolumeManager : MonoBehaviour
{
    // les objets qui sont taggés musique ou son
    private List<GameObject> musicObjects;
    private List<GameObject> sfxObjects;
    
    void Start()
    {
        // on récupère tout les objets taggés de la scène
        musicObjects = GameObject.FindGameObjectsWithTag("SFX_Music").ToList();
        sfxObjects = GameObject.FindGameObjectsWithTag("SFX_Sound").ToList();
        
        // et on met leur volume au volume existant
        SetVolume(VolumeType.MUSIC, PlayerPrefs.GetFloat("MusicVolume"));
        SetVolume(VolumeType.SOUND, PlayerPrefs.GetFloat("SoundVolume"));
    }
    
    public void SetVolume(VolumeType volume,  float value)
    {
        if (volume == VolumeType.MUSIC)
        {
            foreach (GameObject obj in musicObjects)
            {
                // change le volume de l'audiosource
                AudioSource source = obj.GetComponent<AudioSource>();
                source.volume = value;
            }
        } else if (volume == VolumeType.SOUND)
        {
            foreach (GameObject obj in sfxObjects)
            {
                // change le volume de l'audiosource
                AudioSource source = obj.GetComponent<AudioSource>();
                source.volume = value;
            }
        }
    }
}
