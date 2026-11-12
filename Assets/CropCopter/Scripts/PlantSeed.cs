using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/*
 * Gère l'action du drone quand il plante une graine
 */
public class PlantSeed : MonoBehaviour
{
    // controle de plantation
    public InputActionReference plantAction;
    public ParticleSystem water;
    public ScoreManager scoreManager;
    public GameObject plantableArea;
    public Camera camera;
    public GameObject cantPlaceSound;
    public Image selectedBoxImage;
    
    private GameObject plantPrefab;
    private int plantPrefabScoreMultiplier;
    
    // toutes les plantes disponibles (une seule actuellement)
    public List<GameObject> plantsModels = new List<GameObject>();
    public List<int> plantsScoreMultipliers = new List<int>();
    
    //cooldown pour eviter de spam
    public float maxCooldown = 0.2f;
    private float currentCooldown = 0f;

    private void Start()
    {
        // définit la plante a utiliser (inutile actuellement mais plus tard aura un interet)
        plantPrefab = plantsModels[0];
        plantPrefabScoreMultiplier = plantsScoreMultipliers[0];
        currentCooldown = maxCooldown;
    }

    void OnEnable()
    {
        plantAction.action.performed += OnPlant;
    }

    void OnDisable()
    {
        plantAction.action.performed -= OnPlant;
    }

    void Update()
    {
        currentCooldown -= Time.deltaTime;
    }
    
    void OnPlant(InputAction.CallbackContext ctx)
    {
        if (currentCooldown <= 0)
        {
            // cherche la case la plus proche du drone
            (Vector2, GameObject) closestBox = plantableArea
                .GetComponent<GridManager>()
                .GetClosestBox(new Vector2(transform.position.x, transform.position.z));
            
            // si le drone est pas sur la grille
            if (closestBox.Item1 == new Vector2(-1, -1))
            {
                return;
            }
            
            // si la case est déja occupé par une plante
            if (closestBox.Item2)
            {
                cantPlaceSound.GetComponent<AudioSource>().Play();
                selectedBoxImage.GetComponent<FlashRed>().ImageFlash();
                return;
            }
            
            // création de la graine sous le drone
            GameObject newPlant = Instantiate(
                plantPrefab, 
                new Vector3(transform.position.x, transform.position.y - 3, transform.position.z), 
                transform.rotation
            );
            
            //insertion dans la grille
            plantableArea.GetComponent<GridManager>().SetBox(closestBox.Item1, newPlant);
            
            // assigne les objets nécessaire pour que la plante puisse grandir correctement
            newPlant.GetComponent<PlantGrow>().water = water;
            newPlant.GetComponent<PlantGrow>().scoreManager = scoreManager;
            newPlant.GetComponent<PlantGrow>().camera = camera;
            newPlant.GetComponent<PlantGrow>().gridManager = plantableArea.GetComponent<GridManager>();
            
            // toujours un mais pourra être utile dans le futur
            newPlant.GetComponent<PlantGrow>().SetScoreMultiplier(plantPrefabScoreMultiplier);
            
            currentCooldown = maxCooldown;
        }
    }
}
