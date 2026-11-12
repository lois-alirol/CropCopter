using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Gère les clics des boutons du menu principal
 */
public class MainMenuButtons : MonoBehaviour
{
    public LoadingManager loadingManager;
    public Canvas settingsCanvas;

    void Start()
    {
        settingsCanvas.enabled = false;
        
        // on met le timescale a 1 pour jouer l'anim du drone si le jeu etait en pause
        // avant de quitter
        Time.timeScale = 1;
    }
    
    public void StartGame()
    {
        GetComponent<AudioSource>().Play();
        loadingManager.LoadScene(SceneRegistry.GAME);
    }
    
    public void Settings()
    {
        GetComponent<AudioSource>().Play();
        settingsCanvas.enabled = true;
    }
    
    public void QuitGame()
    {
        GetComponent<AudioSource>().Play();
        Application.Quit();
    }
}
