using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public LoadingManager loadingManager;
    public Canvas settingsCanvas;

    void Start()
    {
        settingsCanvas.enabled = false;
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
