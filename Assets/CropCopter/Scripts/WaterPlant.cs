using UnityEngine;
using UnityEngine.InputSystem;

public class WaterPlant : MonoBehaviour
{
    public InputActionReference waterAction;
    public ParticleSystem waterParticles;
    
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
            waterParticles.GetComponent<AudioSource>().Stop();
            return;
        }
        
        if (context.performed)
        {
            waterParticles.GetComponent<AudioSource>().Play();
            waterParticles.Play();
        }
        else if (context.canceled)
        {
            waterParticles.GetComponent<AudioSource>().Stop();
            waterParticles.Stop();
        }
    }
}
