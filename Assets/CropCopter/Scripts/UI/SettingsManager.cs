using UnityEngine;
using UnityEngine.UI;

/*
 * Gère les paramètres et les boutons des paramètres
 */
public class SettingsManager : MonoBehaviour
{
    public Slider musicSlider;
    public Slider soundSlider;
    public VolumeManager volumeManager;
    
    void Start()
    {
		// si les valeurs existent pas = premier chargement du jeu
		// donc on met tous les volumes à 1

        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume", 1f);
    }

    // fonctions qui mettent a jour le volume quand les sliders sont bougés
    
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

    // retourne au menu principal en cachant cette interface 
    
    public void OnBackButton()
    {
        GetComponent<AudioSource>().Play();
        GetComponent<Canvas>().enabled = false;
    }
}
