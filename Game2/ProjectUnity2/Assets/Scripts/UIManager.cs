using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public Image[] lifeHearts;
    public GameObject gameOverPanel;
    public Text scoreText;
    public Text foodText;
    public Text cleanText;

    public void UpdateLives(int lives) {
        for(int i = 0; i < lifeHearts.Length; i++) {
            Image currentHeart = lifeHearts[i];
            currentHeart.color = lives > i ? Color.black : Color.white;
        }
    }

    public void UpdateText(string tag, int coin, int stored) {
        switch (tag) {
            case "Food":
                foodText.text = coin.ToString() + " (" + stored.ToString() + ")";
                break;
            case "Cleaning":
                cleanText.text = coin.ToString() + " (" + stored.ToString() + ")";
                break;
        }
    }


    public void UpdateScore(int score) {
        scoreText.text = "Score: " + score + "m";
    }
}
