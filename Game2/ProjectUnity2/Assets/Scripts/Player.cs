using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //--------------------VARIAVEIS--------------------//
    public float laneSpeed;
    public float speed;
    private int currentLane = 1;
    private Vector3 verticalTargetPosition;

    //-------------------------------------------------//

    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) //mudança de lanes pelas teclas
        {
            ChangeLane(-1); //esquerda
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) //mudança de lanes pelas teclas
        {
            ChangeLane(1); //direita
        }

        Vector3 targetPosition = new Vector3(verticalTargetPosition.x, verticalTargetPosition.y,verticalTargetPosition.z) ; //posição alvo desejada
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, laneSpeed * Time.deltaTime); //passando posição atual, passando posição pra onde deseja ir, passando velocidade até a posição alvo
    }

    private void FixedUpdate() //chamada acada tempo fixo, padrão do tempo: 0,02s
    {
        rb.velocity = Vector3.forward * speed;
    }


    void ChangeLane(int direction)
    {
        int targetLane = currentLane + direction; //lane que ira ser escolhida(esquerda,meio,direita)
        if (targetLane < 0 || targetLane > 2) //verificando valor de targetLane
            return;
        currentLane = targetLane;  //lane atual
        verticalTargetPosition = new Vector3((currentLane - 1), 0, 0); //atualizando vetor
    }
}
