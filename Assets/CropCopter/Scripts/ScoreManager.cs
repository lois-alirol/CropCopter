using System;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestScoreText;
    private int score = 0;
    private int bestScore = 0;

    public void Start()
    {
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
    }
    
    public void Update()
    {
        scoreText.text = "Score:   " + score.ToString("0000");

        if (score > bestScore)
        {
            bestScore = score;
        }
        
        bestScoreText.text = "Best:     " + bestScore.ToString("0000");
    }

    public void OnDisable()
    {
        if (score >= bestScore)
        {
            PlayerPrefs.SetInt("BestScore", score);
        }
    }
    
    public void AddScore(int multiplier = 1)
    {
        score+= 1 * multiplier;
    }
    
    public void RemoveScore(int multiplier = 2)
    {
        score-= 1 * multiplier;
    }

    public int GetScore()
    {
        return score;
    }
}