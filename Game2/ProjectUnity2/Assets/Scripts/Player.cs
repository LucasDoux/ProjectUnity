using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    #region Variables

    public float speed;
    public float minSpeed = 10f;
    public float maxSpeed = 30f;

    public float laneSpeed;
    private int currentLane = 1;

    public int horizontalInput;
    public int oldHInput;

    public float jumpLength;
    public float jumpHeight;
    private bool jumping = false;
    private float jumpStart;

    public float slideLength;
    private bool sliding = false;
    private float slideStart;

    private int currentLife;
    public int maxLife = 3;

    private int foodCoins;
    private int cleanCoins;
    private int storedFood;
    private int storedClean;
    private float score;

    public float invincibleTime;
    private bool invincible = false;
    private int blinkingValue;
    public GameObject model; //Usado para blinkar o player se estiver usando outro asset (Vai ser nosso caso)
    public RectTransform invincibleImageRect;
    private float defaultInvincibleImageLength;

    private Animator anim;
    private Rigidbody rb;
    private BoxCollider boxCollider;
    private Vector3 verticalTargetPosition;
    private Vector3 boxColliderSize;
    
    private UIManager uiManager;

    #endregion

    #region Unity Events

    void Start() {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        boxColliderSize = boxCollider.size;
        anim.Play("runStart");

        currentLife = maxLife;
        speed = minSpeed;

        blinkingValue = Shader.PropertyToID("_BlinkingValue");
        uiManager = FindObjectOfType<UIManager>();

        horizontalInput = 0;
        oldHInput = horizontalInput;

        storedFood = 0;
        storedClean = 0;

        defaultInvincibleImageLength = invincibleImageRect.sizeDelta.x;
        invincibleImageRect.sizeDelta = new Vector2(0,invincibleImageRect.sizeDelta.y);
    }

    // Update is called once per frame
    void Update() {
        score += Time.deltaTime * speed;
        uiManager.UpdateScore((int)score);

        horizontalInput = Convert.ToInt32(Input.GetAxisRaw("Horizontal"));

        if (horizontalInput != oldHInput) {
            ChangeLane(horizontalInput);
        }

        oldHInput = horizontalInput;

        if (Input.GetKeyDown(KeyCode.UpArrow) && !jumping) {
            StartJump();
        } else if (Input.GetKeyDown(KeyCode.DownArrow) && !jumping && !sliding) {
            StartSlide();
        }

        if (jumping) {
            float ratio = (transform.position.z - jumpStart) / jumpLength; //controla a porporção do pulo

            if (ratio >= 1f) {
                jumping = false; //pulo acabou
                anim.SetBool("Jumping", false);
            } else {
                verticalTargetPosition.y = Mathf.Sin(ratio * Mathf.PI) * jumpHeight;
            }
        } else {
            verticalTargetPosition.y = Mathf.MoveTowards(verticalTargetPosition.y, 0, 5 * Time.deltaTime);//atualizando para onnde deseja ir
        }

        if (sliding) {
            float ratio = (transform.position.z - slideStart) / slideLength; //verificando porporção do slde

            if(ratio >= 1) {
                sliding = false;
                anim.SetBool("Sliding", false);
                boxCollider.size = boxColliderSize;
            }
        }

        Vector3 targetPosition = new Vector3(verticalTargetPosition.x, verticalTargetPosition.y , transform.position.z ); //posição alvo desejada
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, laneSpeed * Time.deltaTime);  //passando posição atual, passando posição pra onde deseja ir, passando velocidade até a posição alvo
    }
    

    private void FixedUpdate() {
        rb.velocity = Vector3.forward * speed;
    }

    #endregion

    #region Functions

    void ChangeLane(int direction) {
        int targetLane = currentLane + direction; //lane que ira ser escolhida(esquerda,meio,direita)
        if (targetLane < 0 || targetLane > 2) //verificando valor de targetLane
            return;
        currentLane = targetLane; //lane atual
        verticalTargetPosition = new Vector3((currentLane - 1), 0 ,0); //atualizando vetor
    }


    void StartJump() {
        jumpStart = transform.position.z;
        anim.SetFloat("JumpSpeed", speed / jumpLength); //animação vai ser a velocidade divido pelo tamanho do pulo
        anim.SetBool("Jumping", true);
        jumping = true; //controla o pulo
    }


    void StartSlide() {
        slideStart = transform.position.z;
        anim.SetFloat("JumpSpeed", speed / slideLength);
        anim.SetBool("Sliding", true);
        Vector3 newSize = boxCollider.size; //diminuir o tamanho da box colider quando deslizar
        newSize.y = newSize.y / 2;
        boxCollider.size = newSize;
        sliding = true;
    }

    private void OnTriggerEnter(Collider other) {

        if (other.CompareTag("Food")) {
            foodCoins++; //Sobe a quantidade de coins coletados em 1
            uiManager.UpdateText("Food", foodCoins, storedFood); //Atualiza a tela com o número atual de coins
            other.transform.parent.gameObject.SetActive(false); //Desativa a colisão com o coin depois de já ter colidido
        } else if (other.CompareTag("Cleaning")) {
            cleanCoins++;
            uiManager.UpdateText("Cleaning", cleanCoins, storedClean);
            other.transform.parent.gameObject.SetActive(false);
        } else if (other.CompareTag("Mask")) {
            StartCoroutine(Blinking(10, false));
            other.transform.parent.gameObject.SetActive(false);
        } else if (other.CompareTag("Checkpoint")) {
            storedFood += foodCoins;
            foodCoins = 0;
            storedClean += cleanCoins;
            cleanCoins = 0;
            uiManager.UpdateText("Food", foodCoins, storedFood);
            uiManager.UpdateText("Cleaning", cleanCoins, storedClean);
        }


        if(invincible)
            return;

        if(other.CompareTag("Obstacle")) {

            currentLife--;
            uiManager.UpdateLives(currentLife);
            anim.SetTrigger("Hit");
            speed = 0.4f * speed;

            if(currentLife <= 0) {
                speed = 0;
                anim.SetBool("Dead", true);
                uiManager.gameOverPanel.SetActive(true);
                Invoke("CallMenu", 2f);
            } else {
                StartCoroutine(Blinking(invincibleTime, true));
            }
        }
    }

    IEnumerator Blinking(float time, bool crash) {
        invincible = true;
        float timer = 0;
        float currentBlink = 1f;
        float lastBlink = 0;
        float blinkPeriod = 0.1f;
        bool enabled = false;

        if (crash) {
            yield return new WaitForSeconds(0.75f);
        }

        //speed = minSpeed;
        if (speed * 0.75f < minSpeed) {
            speed = minSpeed;
        } else {
            speed = speed * 0.75f;
        }

        while (timer < time && invincible)
        {
            invincibleImageRect.sizeDelta = new Vector2(defaultInvincibleImageLength * (1f - timer / time), 
                invincibleImageRect.sizeDelta.y);
            //para utilizar a forma de desativar e ativar o modelo, basta descomentar o que está comentado neste método e a variável model.

            model.SetActive(enabled);
            Shader.SetGlobalFloat(blinkingValue, currentBlink);

            yield return null;

            timer += Time.deltaTime;
            lastBlink += Time.deltaTime;

            if(blinkPeriod < lastBlink) {
                lastBlink = 0;
                currentBlink = 1f - currentBlink;
                enabled = !enabled;
            }
        }
        invincibleImageRect.sizeDelta = new Vector2(0, invincibleImageRect.sizeDelta.y);
        model.SetActive(true);
        Shader.SetGlobalFloat(blinkingValue, 0);
        invincible = false;
    }

    void CallMenu() {
        GameManager.gm.EndRun();
    }

    public void IncreaseSpeed() {
        speed *= 1.15f;
        if(speed >= maxSpeed) {
            speed = maxSpeed;
        }
    }

    #endregion
}
