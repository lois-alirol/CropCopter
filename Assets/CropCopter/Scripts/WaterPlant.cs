using UnityEngine;
using UnityEngine.InputSystem;

public class WaterPlant : MonoBehaviour
{
    public InputActionReference waterAction;
    public ParticleSystem waterParticles;
    public GameObject waterSfx;
    
    void OnEnable()
    {
        waterAction.action.performed += OnWater;
        waterAction.action.canceled += OnWater;
    }

    void OnDisable()
    {
        waterAction.action.performed -= OnWater;
        waterAction.action.canceled -= OnWater;
    }

    public void OnWater(InputAction.CallbackContext context)
    {
        if (Time.timeScale <= 0f)
        {
            waterSfx.GetComponent<AudioSource>().Stop();
            return;
        }
        
        if (context.performed)
        {
            waterSfx.GetComponent<AudioSource>().Play();
            waterParticles.Play();
        }
        else if (context.canceled)
        {
            waterSfx.GetComponent<AudioSource>().Stop();
            waterParticles.Stop();
        }
    }
}
