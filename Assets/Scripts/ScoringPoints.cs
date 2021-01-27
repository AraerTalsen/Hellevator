/*Daniel Greenberg
 * Last Updated: 1/14/20
 */
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//The ScoringPoints class is for recording immediate mining expedition points
public class ScoringPoints : MonoBehaviour
{
    public TextMeshProUGUI score;
    public TextMeshProUGUI highScoreText;
    private int points, highScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        score.text = "Score: " + points;
    }

    //Adds points to the total score
    public void IncrementPoints(int num)
    {
        points += num;
        score.text = "Score: " + points;
    }

    //Sets score back to zero
    public void ResetScore()
    {       
        points = 0;
        score.text = "Score: " + points;
    }

    public void SetHighScore()
    {
        Coins.AddSubtractCoins(points);
        if (points > highScore) highScore = points;
        highScoreText.text = "Highscore: " + highScore;
    }
}
