using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    public GameObject loadingScreenPrefab;
    private LoadingScreenUI loadingUI;
    public void LoadScene(SceneRegistry scene)
    {
        StartCoroutine(LoadSceneAsync(scene));
    }

    IEnumerator LoadSceneAsync(SceneRegistry scene)
    {
        GameObject loadingScreen = Instantiate(loadingScreenPrefab);

        loadingUI = loadingScreen.GetComponent<LoadingScreenUI>();
        AsyncOperation loading = SceneManager.LoadSceneAsync((int)scene);

        loading.allowSceneActivation = false;

        float timer = 0f;
        while (timer < 1f || loading.progress < 0.9f)
        {
            timer += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(timer / 1f);

            loadingUI.UpdateProgress(progress);

            yield return null;
        }

        loading.allowSceneActivation = true;

        while (!loading.isDone)
        {
            yield return null;
        }

        Destroy(loadingScreen);
    }
}