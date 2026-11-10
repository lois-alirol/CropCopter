using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Slider musicSlider;
    public Slider soundSlider;
    public VolumeManager volumeManager;
    
    void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume");
    }

    public void OnMusicSliderMove()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        volumeManager.SetVolume(VolumeType.MUSIC, musicSlider.value);
    }
    
    public void OnSoundSliderMove()
    {
        PlayerPrefs.SetFloat("SoundVolume", soundSlider.value);
        volumeManager.SetVolume(VolumeType.SOUND, soundSlider.value);
    }

    public void OnBackButton()
    {
        GetComponent<AudioSource>().Play();
        GetComponent<Canvas>().enabled = false;
    }
}
