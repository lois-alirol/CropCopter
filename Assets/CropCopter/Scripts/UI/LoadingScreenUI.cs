using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingScreenUI : MonoBehaviour
{
    public Image fillImage;
    public TMP_Text loadingText;

    public void UpdateProgress(float progress)
    {
        progress = Mathf.Clamp01(progress);
        fillImage.fillAmount = progress;
        loadingText.text = "Loading... " + (progress * 100).ToString("F0") + "%";
    }
}