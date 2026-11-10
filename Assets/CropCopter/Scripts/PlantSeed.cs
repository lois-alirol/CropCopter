using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlantSeed : MonoBehaviour
{
    public InputActionReference plantAction;
    public ParticleSystem water;
    public ScoreManager scoreManager;
    public GameObject plantableArea;
    public Camera camera;
    public GameObject cantPlaceSound;
    public Image selectedBoxImage;
    
    private GameObject plantPrefab;
    private int plantPrefabScoreMultiplier;
    
    public List<GameObject> plantsModels = new List<GameObject>();
    public List<int> plantsScoreMultipliers = new List<int>();
    
    public float maxCooldown = 0.2f;
    private float currentCooldown = 0f;

    private void Start()
    {
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
            (Vector2, GameObject) closestBox = plantableArea
                .GetComponent<GridManager>()
                .GetClosestBox(new Vector2(transform.position.x, transform.position.z));
            
            if (closestBox.Item1 == new Vector2(-1, -1))
            {
                return;
            }
            
            if (closestBox.Item2)
            {
                cantPlaceSound.GetComponent<AudioSource>().Play();
                selectedBoxImage.GetComponent<FlashRed>().ImageFlash();
                return;
            }
            
            GameObject newPlant = Instantiate(
                plantPrefab, 
                new Vector3(transform.position.x, transform.position.y - 3, transform.position.z), 
                transform.rotation
            );
            
            plantableArea.GetComponent<GridManager>().SetBox(closestBox.Item1, newPlant);
            
            newPlant.GetComponent<PlantGrow>().water = water;
            newPlant.GetComponent<PlantGrow>().scoreManager = scoreManager;
            newPlant.GetComponent<PlantGrow>().camera = camera;
            newPlant.GetComponent<PlantGrow>().gridManager = plantableArea.GetComponent<GridManager>();
            
            newPlant.GetComponent<PlantGrow>().SetScoreMultiplier(plantPrefabScoreMultiplier);
            
            currentCooldown = maxCooldown;
        }
    }
}
