using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour {

    #region Singleton Implementation

    private static MainUI _instance;
    public static MainUI Instance
    {
        get => _instance;
        set
        {
            if (_instance == null)
                _instance = value;
        }
    }

    #endregion

    #region Parameters

    public Text waveText;
    public Text remainingTimeText;
    public Image orangeSelectedImage;
    public Image blueSelectedImage;
    public RectTransform orangeCooldownAxis;
    public Text blueCount;
    public Image winImage;
    
    #endregion

    #region Control Variables

    private Coroutine orangeCooldownCoroutine;

    #endregion

    #region Unity Events

    private void Awake() {
        _instance = this;
    }

    private void Start() {
        orangeCooldownAxis.rotation = Quaternion.Euler(0, 90, 0);
    }

    #endregion

    #region Functions

    public void SetWave(int wave) {
        waveText.text = wave.ToString();
    }

    public void SetRemainingTimeText(int remainingTime) {
        remainingTimeText.text = remainingTime.ToString();
    }

    public void SetSelectedWeapon(int weapon) {
        if (weapon == 1) {
            orangeSelectedImage.enabled = true;
            blueSelectedImage.enabled = false;
        } else if (weapon == 2) {
            orangeSelectedImage.enabled = false;
            blueSelectedImage.enabled = true;
        }
    }

    public void StartCooldown(float cooldownTime) {
        if (orangeCooldownCoroutine != null)
            StopCoroutine(orangeCooldownCoroutine);

        orangeCooldownCoroutine = StartCoroutine(CooldownCoroutine(cooldownTime));
    }

    public void SetBlueCount(int count) {
        blueCount.text = count.ToString();
    }

    public void Win() {
        Time.timeScale = 0;
        winImage.enabled = true;
    }
    
    private IEnumerator CooldownCoroutine(float cooldownTime) {
        var elapsedTime = 0f;

        while (elapsedTime < cooldownTime) {
            var ratio = elapsedTime / cooldownTime;
            orangeCooldownAxis.rotation = Quaternion.Euler(0, 90 * ratio, 0);
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        
        orangeCooldownAxis.rotation = Quaternion.Euler(0,90,0);
    }

    #endregion
}
