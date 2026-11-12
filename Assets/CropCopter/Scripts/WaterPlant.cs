using UnityEngine;
using UnityEngine.InputSystem;

/*
 * Gère les input pour arroser les plantes
 */
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
        // si le timescale est a 0 alors on arrête de jouer les particules
        if (Time.timeScale <= 0f)
        {
            waterSfx.GetComponent<AudioSource>().Stop();
            return;
        }
        
        //quand on appuie sur clic gauche
        if (context.performed)
        {           
            // stop le son et les particules
            waterSfx.GetComponent<AudioSource>().Play();
            waterParticles.Play();
        }
        
        // quand on arrete de maintenir
        else if (context.canceled)
        {
            // stop le son et les particules
            waterSfx.GetComponent<AudioSource>().Stop();
            waterParticles.Stop();
        }
    }
}
