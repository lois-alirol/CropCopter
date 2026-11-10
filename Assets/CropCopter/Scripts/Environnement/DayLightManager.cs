using UnityEngine;

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
        if (currentTime < 0f)
        {
            currentTime = cycleDuration;
        }
        else
        {
            currentTime -= Time.deltaTime;
        }
        
        transform.rotation = Quaternion.Euler(new Vector3(currentTime * 360 / cycleDuration, -30, 0));
        
        Material skybox = betweenSkybox;

        if (currentTime < cycleDuration * 0.9f && currentTime > cycleDuration * 0.6f)
        {
            skybox = daySkybox;
        }
        else if (currentTime < cycleDuration * 0.4f && currentTime > cycleDuration * 0.1f)
        {
            skybox = nightSkybox;
        }

        if (RenderSettings.skybox != skybox)
        {
            RenderSettings.skybox = skybox;
        }
    }
}