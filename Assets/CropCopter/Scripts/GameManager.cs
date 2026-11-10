using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        if (isRunning && currentTime <= maxTime)
        {
            currentTime += Time.deltaTime;
            
            float secondsLeft = maxTime - currentTime;
            float percentage = secondsLeft / 60f;
            
            int minutes = Mathf.FloorToInt(secondsLeft / 60);
            int seconds = Mathf.FloorToInt(secondsLeft % 60);

            timeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
            timeImage.fillAmount = percentage;
            score = drone.GetComponent<ScoreManager>().GetScore();
        }
        else if (isRunning && currentTime >= maxTime)
        {
            gameMusic.GetComponent<AudioSource>().Stop();
            music.GetComponent<AudioSource>().PlayDelayed(2f);
            endSound.GetComponent<AudioSource>().Play();
            
            isRunning = false;
            gameOver = true;
            Time.timeScale = 0f;
            
            timeText.text =  "00:00";
            
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
            Destroy(drone);
        } else if (gameOver && currentTime >= maxTime)
        {
            endGameUi.gameObject.SetActive(true);
            endGameText.text = "Your Score: " + score.ToString("0000");
        }
    }

    public void SetMaxTime(float time)
    {
        maxTime = time;
    }

    IEnumerator StartGame()
    {
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
        
        background.gameObject.SetActive(false);
        drone.SetActive(true);
        beforeStartText.text = "";
        Time.timeScale = 1f;
        isRunning = true;
    }
}
