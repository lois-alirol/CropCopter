using UnityEngine;


/*
 * Gère le cycle jour nuit de la skybox
 * 
 */
public class DayLightManager : MonoBehaviour
{
    public float cycleDuration = 600f;
    public float startTime = 0f;

    public Material daySkybox;
    public Material betweenSkybox;
    public Material nightSkybox;
    
    private float currentTime = 0f;
    
    void Start()
    {
        currentTime = startTime;
    }
    
    void Update() 
    {
        // repars à 0 ou retire le temps
        if (currentTime < 0f)
        {
            currentTime = cycleDuration;
        }
        else
        {
            currentTime -= Time.deltaTime;
        }
        
        // calcule la rotation 
        // fonction euler => permet de passer d'angles en degré en un quaternion
        transform.rotation = Quaternion.Euler(new Vector3(currentTime * 360 / cycleDuration, -30, 0));
        
        Material skybox = betweenSkybox;

        // passe la skybox au jour
        if (currentTime < cycleDuration * 0.9f && currentTime > cycleDuration * 0.6f)
        {
            skybox = daySkybox;
        }
        //passe la skybox a la nuit
        else if (currentTime < cycleDuration * 0.4f && currentTime > cycleDuration * 0.1f)
        {
            skybox = nightSkybox;
        }

        // ne modifie la skybox que quand la variable skybox est différente de celle appliquée
        if (RenderSettings.skybox != skybox)
        {
            RenderSettings.skybox = skybox;
        }
    }
}