using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/*
 * Gère l'écran de chargement ainsi que les changement de scène
 */
public class LoadingManager : MonoBehaviour
{
    public GameObject loadingScreenPrefab;
    private LoadingScreenUI loadingUI;
    
    // démarre la coroutine quand un changement de scèene est demandé
    public void LoadScene(SceneRegistry scene)
    {
        StartCoroutine(LoadSceneAsync(scene));
    }

    IEnumerator LoadSceneAsync(SceneRegistry scene)
    {
        // affiche l'écran de chargement
        GameObject loadingScreen = Instantiate(loadingScreenPrefab);

        loadingUI = loadingScreen.GetComponent<LoadingScreenUI>();
        AsyncOperation loading = SceneManager.LoadSceneAsync((int)scene);

        loading.allowSceneActivation = false;

        float timer = 0f;
        while (timer < 1f || loading.progress < 0.9f)
        {
            // unscaled delta time car le jeu pourrait etre pausé et donc timescale à 0
            timer += Time.unscaledDeltaTime;
            //garde la progression entre 0 et 1
            float progress = Mathf.Clamp01(timer / 1f);

            //met a jour l'ui
            loadingUI.UpdateProgress(progress);

            yield return null;
        }

        //apres que le chargement coté interface ait fini on autorise a afficher la scene chargée
        loading.allowSceneActivation = true;

        while (!loading.isDone)
        {
            yield return null;
        }

        // on attend que le chargement soit fini et on détruit l'écran de chargement
        Destroy(loadingScreen);
    }
}