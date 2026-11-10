using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuButtons : MonoBehaviour
{
    public ScoreManager scoreManager;
    public InputActionReference pauseAction;
    public LoadingManager loadingManager;
    private bool isPaused = false;

    void OnEnable()
    {
        GetComponent<Canvas>().enabled = false;
        pauseAction.action.performed += OnPause;
    }

    void OnDisable()
    {
        pauseAction.action.performed -= OnPause;
    }

    void OnPause(InputAction.CallbackContext ctx)
    {
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