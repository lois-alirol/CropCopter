using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class FlashRed : MonoBehaviour
{
    private Color originalImageColor;
    
    void Start()
    {
        originalImageColor = gameObject.GetComponent<Image>().color;
    }
    
    public void ImageFlash()
    {
        StartCoroutine(ImageFlashCoroutine());
    }
    
    IEnumerator ImageFlashCoroutine()
    {
        gameObject.GetComponent<Image>().color = new Color(207 / 255f, 48 / 255f, 48 / 255f);
        
        yield return new WaitForSeconds(0.5f);
        
        gameObject.GetComponent<Image>().color = originalImageColor;
    }
}