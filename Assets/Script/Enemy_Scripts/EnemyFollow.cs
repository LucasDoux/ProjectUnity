using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using System.Linq;

public class EnemyFollow : MonoBehaviour
{
    public float knockbackForce;

    private bool canWalk;
    public float speed;

    public Player player;


    private Rigidbody2D rb;
    private Transform originalTarget;
    public Transform targetFollow;
    
    void Start()
    {
        player = FindObjectOfType<Player>().GetComponent<Player>();

        originalTarget = GameObject.FindGameObjectWithTag("Finish").transform; // assim que começar ele ira logo atras da tag "Player"(Object)
        targetFollow = originalTarget;
        canWalk = true;
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        if (targetFollow == null) {
            if (originalTarget == null) {
                originalTarget = GameObject.FindGameObjectWithTag("Finish").transform;
            }

            targetFollow = originalTarget;
        }
            

        if (!canWalk)
            return;

        if(Vector2.Distance(transform.position, targetFollow.position)> 0.1f) //caso ele encontre o player ele fica a 0.3f de distancia do player
        transform.position = Vector2.MoveTowards(transform.position, targetFollow.position, speed * Time.deltaTime);//calculando a posição em que o player se move para segui-lo

        //var deltaVector = targetFollow.position - transform.position;
        //var directionDelta = deltaVector.normalized;

        //var newPosition = transform.position + (directionDelta * speed/100);
        //transform.position = newPosition;

    }

    //private void OnCollisionStay2D(Collision2D collision) {
    //    Debug.Log(collision.gameObject.tag);
    //}

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Arma")) {
            var playerPosition = player.transform.position;
            var deltaPosition = transform.position - playerPosition;

            var absDelta = deltaPosition.normalized;

            rb.AddForce(absDelta * knockbackForce);

            StartCoroutine(LockWalk(1.5f));
            //StartCoroutine(KnockbackCoroutine());
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        //Debug.Log("Indo");

        if (targetFollow != collision.transform && collision.CompareTag("Food")) {
            targetFollow = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Food")) {
            targetFollow = originalTarget;
        }
    }

    private IEnumerator LockWalk(float time) {
        canWalk = false;

        yield return new WaitForSeconds(time);

        canWalk = true;
    }
}
