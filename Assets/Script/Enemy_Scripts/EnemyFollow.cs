using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using System.Linq;

public class EnemyFollow : MonoBehaviour
{

    public float speed;

    public Player player;

    private Transform originalTarget;
    public Transform targetFollow;
    
    void Start()
    {
        player = FindObjectOfType<Player>().GetComponent<Player>();

        originalTarget = GameObject.FindGameObjectWithTag("Finish").transform; // assim que começar ele ira logo atras da tag "Player"(Object)
        targetFollow = originalTarget;
    }
    
    void Update()
    {
        if(Vector2.Distance(transform.position, targetFollow.position)> 0.3f) //caso ele encontre o player ele fica a 0.3f de distancia do player
        transform.position = Vector2.MoveTowards(transform.position, targetFollow.position, speed * Time.deltaTime);//calculando a posição em que o player se move para segui-lo
    }

    private void OnTriggerStay2D(Collider2D collision) {
        Debug.Log("Indo");

        if (targetFollow != collision.transform && collision.CompareTag("Food")) {
            targetFollow = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Food")) {
            targetFollow = originalTarget;
        }
    }
}
