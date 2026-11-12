using System;
using TMPro;
using UnityEngine;

/*
 * S'occupe du score et de l'interface du score
 */
public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestScoreText;
    private int score = 0;
    private int bestScore = 0;

    public void Start()
    {
        // charge le meilleur score ou met 0 si pas existant 
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
    }
    
    public void Update()
    {
        // met a jour le score
        scoreText.text = "Score:   " + score.ToString("0000");

        // si le score est mieux que le meilleur score alors on met le meilleur score au score actuel 
        if (score > bestScore)
        {
            bestScore = score;
        }
        
        // met a jour le meilleur score
        bestScoreText.text = "Best:     " + bestScore.ToString("0000");
    }

    public void OnDisable()
    {
        // si le score est mieux que le meilleur score on le sauvegarde a la fin de la partie
        if (score >= bestScore)
        {
            PlayerPrefs.SetInt("BestScore", score);
        }
    }
    
    /*
     * Ajoute du score quand une plante est récupérée
     */
    public void AddScore(int multiplier = 1)
    {
        score+= 1 * multiplier;
    }
    
    /*
     * Retire du score quand une plante explose
     */
    public void RemoveScore(int multiplier = 2)
    {
        score-= 1 * multiplier;
    }

    // retourne le score
    public int GetScore()
    {
        return score;
    }
}