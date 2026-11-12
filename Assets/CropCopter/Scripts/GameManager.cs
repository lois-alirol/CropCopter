using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
 * Gère les parties ainsi que l'interface du temps et de la fin de partie
 */
public class GameManager : MonoBehaviour
{
    public float maxTime = 60f;
    private float currentTime = 0f;
    
    private bool isRunning = false;
    private bool gameOver = false;
    
    public GameObject drone;
    public GameObject grid;
    public Image background;
    public TextMeshProUGUI timeText;
    public Image timeImage;
    public TextMeshProUGUI beforeStartText;
    public Canvas endGameUi;
    public TextMeshProUGUI endGameText;
    public GameObject gameMusic;
    public GameObject music;
    public GameObject endSound;
    
    private int score = 0;
    
    void Start()
    {
        StartCoroutine(StartGame());
    }

    void Update()
    {
        // partie en cours et temps actuel < au temps max de la partie
        if (isRunning && currentTime <= maxTime)
        {
            currentTime += Time.deltaTime;
            
            float secondsLeft = maxTime - currentTime;
            float percentage = secondsLeft / 60f;
            
            int minutes = Mathf.FloorToInt(secondsLeft / 60);
            int seconds = Mathf.FloorToInt(secondsLeft % 60);

            // change le texte de l'interface du temps
            timeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
            // change le remplissage du cercle
            timeImage.fillAmount = percentage;
            
            //stocke temporariement le score au cas ou le jeu s'arrete la frame d'après
            score = drone.GetComponent<ScoreManager>().GetScore();
        }
        //si le jeu tourne mais que le temps es tdépassé
        else if (isRunning && currentTime >= maxTime)
        { 
            // stop de la musique de jeu
            gameMusic.GetComponent<AudioSource>().Stop();
            // joue la musique de menu avec un délait de2secondes
            music.GetComponent<AudioSource>().PlayDelayed(2f);
            // joue le son de fin de partie
            endSound.GetComponent<AudioSource>().Play();
            
            // le jeu ne tourne plus et la partie est "officiellement" finie
            // arrete aussi le temps
            isRunning = false;
            gameOver = true;
            Time.timeScale = 0f;
            
            // met le texte a 0 sinon ca serait -1:-1
            timeText.text =  "00:00";
            
            // parcours la grille et détruis chaque plante dans la grille
            foreach (var row in grid.GetComponent<GridManager>().GetGrid())
            {
                foreach (var box in row)
                {
                    if (box.Item2)
                    {
                        Destroy(box.Item2);
                    }
                }
            }
            // détruis le drone pour ne pas laisser le joueur le bouger
            Destroy(drone);
        //si la game est finie pour de vraie
        } else if (gameOver && currentTime >= maxTime)
        {
            // on affiche l'interface de fin aisin que le score du joueur
            endGameUi.gameObject.SetActive(true);
            endGameText.text = "Your Score: " + score.ToString("0000");
        }
    }

    /*
     * Fonction qui change le temps max pour feature future
     */
    public void SetMaxTime(float time)
    {
        maxTime = time;
    }

    /*
     * S'occupe d'afficher le compteur de démarrage du jeu et de démarrer la partie
     */
    IEnumerator StartGame()
    {
        // retire l'ui de fin et désactive le drone, le temps et met le compteur à 3
        endGameUi.gameObject.SetActive(false);
        drone.SetActive(false);
        Time.timeScale = 0f;
        beforeStartText.text = "3";
        
        yield return new WaitForSeconds(1f);
        
        beforeStartText.text = "2";
        yield return new WaitForSeconds(1f);
        
        beforeStartText.text = "1";
        yield return new WaitForSeconds(1f);
        
        beforeStartText.text = "GO";
        yield return new WaitForSeconds(0.2f);
        
        // désactive le fond noir pour laisser apparaitre la scene de jeu
        background.gameObject.SetActive(false);
        //active le drone et "retire" le texte du compteur
        drone.SetActive(true);
        beforeStartText.text = "";
        // met le temps en mode normal
        Time.timeScale = 1f;
        //indique que la partie tourne a partir de maintenant
        isRunning = true;
    }
}
