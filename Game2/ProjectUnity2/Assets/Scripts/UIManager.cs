﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public Image[] lifeHearts;
    public Text coinText;
    public GameObject gameOverPanel;
    public Text scoreText;

    public void UpdateLives(int lifes) {
        for(int i = 0; i < lifeHearts.Length; i++) {
            Image currentHeart = lifeHearts[i];
            currentHeart.color = lifes > i ? Color.white : Color.black;
        }
    }

    public void UpdateCoins(int coin) {
        coinText.text = coin.ToString();
    }

    public void UpdateScore(int score) {
        scoreText.text = "Score: " + score + "m";
    }
}
