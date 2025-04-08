using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    public TMP_Text scoreText;

    private void Update()
    {
        float score = ScoreManager.instance.GetScore();
        scoreText.text = "Total Score: " + ScoreManager.instance.GetScore().ToString();
    }

}
