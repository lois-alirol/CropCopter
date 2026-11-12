using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/*
 * Les différents état des plantes
 */
public enum PlantState
{
    FALLING,
    SEEDING,
    GROWING,
    RIPING
}

/*
 * Gère l'éolution d'une plante
 */
public class PlantGrow : MonoBehaviour
{
    // temps de vie aléatoire
    private float seedingTime;
    private float readyTime;
    private float explosionTime;

    // par défaut une plante tombe
    private PlantState currentState = PlantState.FALLING;
    private float currentCooldown = 0f;
    
    public ParticleSystem water;
    public ScoreManager scoreManager;
    private Animator growAnimator;
    public Image lifeBarImage;
    public Camera camera;
    public Canvas lifeBarUi;
    public GameObject model;
    public ParticleSystem explodeParticles;
    public GridManager gridManager;
    
    public GameObject explodeSound;
    public GameObject harvestSound;
    
    // les différents modèles pour chaque stade
    public GameObject stage0;
    public GameObject stage1;
    public GameObject stage2;
    public GameObject stage3;
    
    private bool isHarvested = false;
    private int scoreMultiplier = 1;
    
    // les temps min et max pour chaque stade
    public float minSeedingTime = 5f;
    public float maxSeedingTime = 15f;
    
    public float minReadyTime = 3f;
    public float maxReadyTime = 8f;
    
    public float minExplosionTime = 3f;
    public float maxExplosionTime = 8f;
    
    private void Start()
    {
        seedingTime = Random.Range(minSeedingTime, maxSeedingTime);
        readyTime = Random.Range(minReadyTime, maxReadyTime);

        currentCooldown = seedingTime;

        // lance l'animation de grandissement
        growAnimator = model.GetComponent<Animator>();
        growAnimator.SetTrigger("Grow");

        // par défaut barre de vie au max
        lifeBarImage.fillAmount = 1f;
        
        // on affiche que le stade 0 (la graine qui tombe)
        stage1.SetActive(false);
        stage2.SetActive(false);
        stage3.SetActive(false);
    }

    void Update()
    {
        if (currentState == PlantState.FALLING && gameObject.transform.position.y < 0.3f)
        {
            // on recentre la plante au millieu de la case la plus proche
            (Vector2, GameObject) box = gridManager.GetClosestBox(new Vector2(transform.position.x, transform.position.z));
            transform.position = new Vector3(box.Item1.x, gameObject.transform.position.y , box.Item1.y);
            
            // on désactive la graine et on affiche le stade 1
            stage0.SetActive(false);
            stage1.SetActive(true);
            stage1.transform.localScale = stage0.transform.localScale;
            currentState = PlantState.SEEDING;
            
            // on freeze toutes les positions et rotations
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        
        if (currentCooldown <= 0 && currentState == PlantState.RIPING)
        {
            // explosion d'une plante
            scoreManager.RemoveScore();
            
            GameObject sfx = Instantiate(explodeSound, camera.transform.position, camera.transform.rotation);
            sfx.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SoundVolume");
            sfx.GetComponent<AudioSource>().Play();
            
            ParticleSystem fx = Instantiate(explodeParticles, transform.position, Quaternion.identity);
            fx.Play();
            
            Destroy(fx, 5f);
            Destroy(sfx, 5f);
            Destroy(gameObject);
        }
        else if (currentCooldown <= 0 && currentState == PlantState.SEEDING)
        {
            currentCooldown = readyTime;
            stage1.SetActive(false);
            stage2.SetActive(true);
            stage2.transform.localScale = stage1.transform.localScale;
            
            currentState = PlantState.GROWING;

            // on lance l'animation qui grandit encore plus
            growAnimator.SetFloat("GrowProgress", 1f);
            growAnimator.SetTrigger("GrowMore");
        }
        else if (currentCooldown <= 0 && currentState == PlantState.GROWING)
        {
            stage2.SetActive(false);
            stage3.SetActive(true);

            currentState = PlantState.RIPING;

            explosionTime = Random.Range(minExplosionTime, maxExplosionTime);
            currentCooldown = explosionTime;
        }
        
        
        // mise a jour de la durée de l'animation comme le tmeps est aléatoire
        if (currentState != PlantState.RIPING)
        {
            float duration = currentState == PlantState.SEEDING ? seedingTime : readyTime;
            float progress = Mathf.Clamp01(1f - currentCooldown / duration);

            string param = currentState == PlantState.SEEDING ? "GrowProgress" : "GrowMoreProgress";
            
            growAnimator.SetFloat(param, progress);
        }

        
        // mise a jour pour chaque état de la barre de vie
        if (currentState == PlantState.SEEDING)
        {
            lifeBarImage.color = new Color32(255, 132, 0, 255);
            lifeBarImage.fillAmount = currentCooldown / seedingTime;
        }
        else if (currentState == PlantState.GROWING)
        {
            lifeBarImage.color = new Color32(77, 255, 0, 255);
            lifeBarImage.fillAmount = currentCooldown / readyTime;
        }
        else if (currentState == PlantState.RIPING)
        {
            lifeBarImage.color = Color.red;
            lifeBarImage.fillAmount = currentCooldown / explosionTime;
        }
        
        
        currentCooldown -= Time.deltaTime;
        
        // la barre de vie regarde la camera
        lifeBarUi.transform.LookAt(camera.transform);
    }

    public void OnTriggerStay(Collider other)
    {
        // si c'est de l'eau et que l'animation se joue (=le joueur appuie sur clic gauche)
        if (other.gameObject.tag.Equals("Water") && water.isPlaying)
        {
            currentCooldown -= 0.05f;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        //récupération d'une plante si elle n'est pas morte déja et pas récupérée pour eviter la dupli de points
        if (other.gameObject.tag.Equals("Drone") && currentState == PlantState.RIPING && !isHarvested)
        {
            GameObject sfx = Instantiate(harvestSound, camera.transform.position, camera.transform.rotation);
            sfx.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SoundVolume");
            sfx.GetComponent<AudioSource>().Play();
            isHarvested = true;
            
            Destroy(gameObject);
            Destroy(sfx, 5f);
            scoreManager.AddScore();
        }
    }

    // change le multiplicateur de score
    public void SetScoreMultiplier(int scoreMultiplier)
    {
        this.scoreMultiplier = scoreMultiplier;
    }
}