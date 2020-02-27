using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{

    public float speed;

    private Transform playerPos;
    
    void Awake()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform; // assim que começar ele ira logo atras da tag "Player"(Object)
    }

    
    void Update()
    {
        if(Vector2.Distance(transform.position, playerPos.position)> 0.3f) //caso ele encontre o player ele fica a 0.3f de distancia do player
        transform.position = Vector2.MoveTowards(transform.position, playerPos.position, speed * Time.deltaTime);//calculando a posição em que o player se move para segui-lo
    }
}
