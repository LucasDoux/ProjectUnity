﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{

    public GameObject bullet;

    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    public Camera cam;
    private Animator legAnim;

    Vector2 movement;
    Vector2 mousePos;

     void Awake()
    {
        legAnim = transform.GetChild(2).GetComponent<Animator>();
        
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        //Shoot Function
        if (Input.GetMouseButtonDown(0)) {
            var bulletObject = Instantiate(bullet, transform.position, Quaternion.identity); // o que é isso Quaternion.identity
            //transform.rotation *= Quaternion.Euler(90, 0, 0); Rotate 90deg
        }

        //Bounds = limitação da tela no personagem(paredes invisiveis)
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -3.3f, 3.3f), Mathf.Clamp(transform.position.y, -2.4f, 2.4f));
                                                     //POSIÇÃO LIMITE CAMERA EM X                    POSIÇÃO LIMITE CAMERA EM Y
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        Vector2 lookDir = mousePos - rb.position;

        //Atan2 = Retorna o ângulo em radianos cujo Tan é y/x.
        //Valor de retorno é o ângulo entre o eixo x e um vetor 2D começando em zero e terminando em(x, y).

        //Constante de conversão de radianos para graus (somente leitura).

        //Isso é igual a 360 / (PI * 2).

        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;

        if (movement == Vector2.zero)
            legAnim.SetBool("Moving", false);
        else
            legAnim.SetBool("Moving", true);
    }



}