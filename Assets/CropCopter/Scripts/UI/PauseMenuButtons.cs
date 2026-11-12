using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/*
 * Gère les clics des boutons de l'interface de pause
 */
public class PauseMenuButtons : MonoBehaviour
{
    public ScoreManager scoreManager;
    public InputActionReference pauseAction;
    public LoadingManager loadingManager;
    private bool isPaused = false;

    void OnEnable()
    {
        //par défaut invisible
        GetComponent<Canvas>().enabled = false;
        pauseAction.action.performed += OnPause;
    }

    void OnDisable()
    {
        pauseAction.action.performed -= OnPause;
    }

    void OnPause(InputAction.CallbackContext ctx)
    {
        // si le jeu est en pause on désactive et change l'état du bool
        //sinon on le met a true et on affiche l'ui
        if (isPaused)
        {
            GetComponent<Canvas>().enabled = false;
            isPaused = false;
        }
        else
        {
            GetComponent<Canvas>().enabled = true;
            isPaused = true;
        }
    }
    
    public void Update()
    {
        // on change le timescale a 0 si c'est en pause pour que tout s'arrete dans le jeu
        if (isPaused)
        {
            Time.timeScale = 0;
        } 
        else
        {
            Time.timeScale = 1;
        }
    }
    
    public void ResumeGame()
    {
        GetComponent<AudioSource>().Play();
        GetComponent<Canvas>().enabled = false;
        isPaused = false;
    }

    public void QuitGame()
    {
        GetComponent<AudioSource>().Play();
        Time.timeScale = 1;
        scoreManager.gameObject.SetActive(false);
        loadingManager.LoadScene(SceneRegistry.MAIN_MENU);
    }
}