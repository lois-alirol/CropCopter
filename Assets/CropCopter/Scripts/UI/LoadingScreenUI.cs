using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * Gère l'interface de chargement notemment la bar e progression
 */
public class LoadingScreenUI : MonoBehaviour
{
    public Image fillImage;
    public TMP_Text loadingText;

    public void UpdateProgress(float progress)
    {
        // clamp pour garder la valeur entre 0 et 1
        progress = Mathf.Clamp01(progress);
        fillImage.fillAmount = progress;
        
        // met a jour le texte
        loadingText.text = "Loading... " + (progress * 100).ToString("F0") + "%";
    }
}