using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image[] lifeHearts;
    public Text coinText;

    public void UpdateLifes(int lifes)
    {
        for(int i = 0; i < lifeHearts.Length; i++)
        {
            Image currentHeart = lifeHearts[i];
            currentHeart.color = lifes > i ? Color.white : Color.black;
        }
    }

    public void UpdateCoins(int coin)
    {
        coinText.text = coin.ToString();
    }
}
