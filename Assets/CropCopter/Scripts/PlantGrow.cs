using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum PlantState
{
    FALLING,
    SEEDING,
    GROWING,
    RIPING
}

public class PlantGrow : MonoBehaviour
{
    private float seedingTime;
    private float readyTime;
    private float explosionTime;

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
    
    public GameObject stage0;
    public GameObject stage1;
    public GameObject stage2;
    public GameObject stage3;
    
    private bool isHarvested = false;
    private int scoreMultiplier = 1;
    
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

        growAnimator = model.GetComponent<Animator>();
        growAnimator.SetTrigger("Grow");

        lifeBarImage.fillAmount = 1f;
        
        stage1.SetActive(false);
        stage2.SetActive(false);
        stage3.SetActive(false);
    }

    void Update()
    {
        if (currentState == PlantState.FALLING && gameObject.transform.position.y < 0.3f)
        {
            (Vector2, GameObject) box = gridManager.GetClosestBox(new Vector2(transform.position.x, transform.position.z));
            transform.position = new Vector3(box.Item1.x, gameObject.transform.position.y , box.Item1.y);
            
            stage0.SetActive(false);
            stage1.SetActive(true);
            stage1.transform.localScale = stage0.transform.localScale;
            currentState = PlantState.SEEDING;
            
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        
        if (currentCooldown <= 0 && currentState == PlantState.RIPING)
        {
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
        
        
        
        if (currentState != PlantState.RIPING)
        {
            float duration = currentState == PlantState.SEEDING ? seedingTime : readyTime;
            float progress = Mathf.Clamp01(1f - currentCooldown / duration);

            string param = currentState == PlantState.SEEDING ? "GrowProgress" : "GrowMoreProgress";
            
            growAnimator.SetFloat(param, progress);
        }

        
        
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
        lifeBarUi.transform.LookAt(camera.transform);
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Water") && water.isPlaying)
        {
            currentCooldown -= 0.05f;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
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

    public void SetScoreMultiplier(int scoreMultiplier)
    {
        this.scoreMultiplier = scoreMultiplier;
    }
}