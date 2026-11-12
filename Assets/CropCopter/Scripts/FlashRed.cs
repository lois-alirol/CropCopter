using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

/*
 * Fais flashé l'interface en rouge quand le joueur essaie de placer une plate sur une case qui
 * a deja une plante plantée
 */
public class FlashRed : MonoBehaviour
{
    private Color originalImageColor;
    
    void Start()
    {
		// on stocke la couleur d'origine
        originalImageColor = gameObject.GetComponent<Image>().color;
    }
    
    public void ImageFlash()
    {
        StartCoroutine(ImageFlashCoroutine());
    }
    
    IEnumerator ImageFlashCoroutine()
    {
        // passe en rouge
        gameObject.GetComponent<Image>().color = new Color(207 / 255f, 48 / 255f, 48 / 255f);
        
        // attend 0.5s
        yield return new WaitForSeconds(0.5f);
        
        // remet la couleur de base
        gameObject.GetComponent<Image>().color = originalImageColor;
    }
}