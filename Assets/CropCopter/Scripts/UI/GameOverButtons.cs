using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Gère les clics des boutons de l'interface de fin de partie
 */
public class GameOverButtons : MonoBehaviour
{
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
