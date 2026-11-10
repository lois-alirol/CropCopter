using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverButtons : MonoBehaviour
{
    public ScoreManager scoreManager;
    public LoadingManager loadingManager;
    
    public void RestartGame()
    {
        loadingManager.LoadScene(SceneRegistry.GAME);
    }

    public void QuitGame()
    {
        loadingManager.LoadScene(SceneRegistry.MAIN_MENU);
    }
}
