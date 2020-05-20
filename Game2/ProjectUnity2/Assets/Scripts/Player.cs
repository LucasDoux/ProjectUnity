using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //--------------------VARIAVEIS--------------------//

    public float speed;
    public float laneSpeed;
    public float jumpLength;
    public float jumpHeight;
    public float slideLength;
    public int maxLife = 3;
    public float minSpeed = 10f;
    public float maxSpeed = 30f;
    public float invencibleTime;
    // public GameObject model; //Usado para blinkar o player se estiver usando outro asset (Vai ser nosso caso)

    private Animator anim;
    private Rigidbody rb;
    private BoxCollider boxCollider;
    private int currentLane = 1;
    private Vector3 verticalTargetPosition;
    private bool jumping = false;
    private float jumpStart;
    private bool sliding = false;
    private float slideStart;
    private Vector3 boxColliderSize;
    private int currentLife;
    private bool invencible = false;
    private int blinkingValue;
    private UIManager uiManager;
    private int coins;
    
    //-------------------------------------------------//
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        boxColliderSize = boxCollider.size;
        anim.Play("runStart");
        currentLife = maxLife;
        speed = minSpeed;
        blinkingValue = Shader.PropertyToID("_BlinkingValue");
        uiManager = FindObjectOfType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))//mudança de lanes pelas teclas
        {
            ChangeLane(-1);//esquerda
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))//mudança de lanes pelas teclas
        {
            ChangeLane(1);//direita
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Slider();
        }


        if (jumping) //verificando se o jumping é verdadeiro
        {
            float ratio = (transform.position.z - jumpStart) / jumpLength; //controla a porporção do pulo
            if(ratio >= 1f)
            {
                jumping = false; //pulo acabou
                anim.SetBool("Jumping", false);
            }
            else
            {
                verticalTargetPosition.y = Mathf.Sin(ratio * Mathf.PI) * jumpHeight;
            }
        }

        else
        {
            verticalTargetPosition.y = Mathf.MoveTowards(verticalTargetPosition.y, 0, 5 * Time.deltaTime);//atualizando para onnde deseja ir
        }

        if (sliding)
        {
            float ratio = (transform.position.z - slideStart) / slideLength; //verificando porporção do slde
            if(ratio >= 1)
            {
                sliding = false;
                anim.SetBool("Sliding", false);
                boxCollider.size = boxColliderSize;
            }
        }

        Vector3 targetPosition = new Vector3(verticalTargetPosition.x, verticalTargetPosition.y , transform.position.z ); //posição alvo desejada
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, laneSpeed * Time.deltaTime);  //passando posição atual, passando posição pra onde deseja ir, passando velocidade até a posição alvo
    }

    

    private void FixedUpdate() //chamada acada tempo fixo, padrão do tempo: 0,02s
    {
        rb.velocity = Vector3.forward * speed;
    }


     void ChangeLane(int direction)
    {
        int targetLane = currentLane + direction; ; //lane que ira ser escolhida(esquerda,meio,direita)
        if (targetLane < 0 || targetLane > 2) //verificando valor de targetLane
            return;
        currentLane = targetLane; //lane atual
        verticalTargetPosition = new Vector3((currentLane - 1), 0 ,0); //atualizando vetor
    }


    void Jump()
    {
        if (!jumping)//verificando se não esta pulando
        {
            jumpStart = transform.position.z;
            anim.SetFloat("JumpSpeed", speed / jumpLength ); //animação vai ser a velocidade divido pelo tamanho do pulo
            anim.SetBool("Jumping", true);
            jumping = true; //controla o pulo
        }
    }


    void Slider()
    {
        if(!jumping && !sliding)//nao permite fazer o slide enquanto está pulando e nem que fique escorregando infinitamente
        {
            slideStart = transform.position.z;
            anim.SetFloat("JumpSpeed", speed / slideLength);
            anim.SetBool("Sliding", true);
            Vector3 newSize = boxCollider.size; //diminuir o tamanho da box colider quando deslizar
            newSize.y = newSize.y / 2;
            boxCollider.size = newSize;
            sliding = true;
        }
    }

    private void OnTriggerEnter(Collider other) {

        if(other.CompareTag("Coin"))
        {
            coins++; //Sobe a quantidade de coins coletados em 1
            uiManager.UpdateCoins(coins); //Atualiza a tela com o número atual de coins
            other.transform.parent.gameObject.SetActive(false); //Desativa a colisão com o coin depois de já ter colidido
        }

        if(invencible)
            return;

        if(other.CompareTag("Obstacle")) {
            currentLife--;
            uiManager.UpdateLifes(currentLife);
            anim.SetTrigger("Hit");
            speed = 0;
            if(currentLife <= 0) {
                //TODO: Game Over
            } else {
                StartCoroutine(Blinking(invencibleTime));
            }
        }
    }

    IEnumerator Blinking(float time) {
        invencible = true;
        float timer = 0;
        float currentBlink = 1f;
        float lastBlink = 0;
        float blinkPeriod = 0.1f;
        // bool enabled = false;
        yield return new WaitForSeconds(1f);
        speed = minSpeed;
        while(timer < time && invencible) {
            //Essa forma provavelmente só irá funcionar nesse shader. Para funcionar noutro
            //é bom desativar e ativar o modelo. (Aula 6)

            //para utilizar a forma de desativar e ativar o modelo, basta descomentar o que está comentado neste método e a variável model.

            // model.SetActive(enabled);
            Shader.SetGlobalFloat(blinkingValue, currentBlink);
            yield return null;
            timer += Time.deltaTime;
            lastBlink += Time.deltaTime;
            if(blinkPeriod < lastBlink) {
                lastBlink = 0;
                currentBlink = 1f - currentBlink;
                // enabled = !enabled;
            }
        }
        // model.SetActive(true);
        Shader.SetGlobalFloat(blinkingValue, 0);
        invencible = false;
    }
}
