using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using System.Linq;

public class EnemyFollow : MonoBehaviour
{
    public float knockbackForce;
    private bool canWalk;
    private bool canLeave;
    private bool isSatisfied;
    public float speed;

    public Player player;

    private Rigidbody2D rb;
    private Transform originalTarget;
    public Transform targetFollow;
    
    void Start()
    {
        player = FindObjectOfType<Player>().GetComponent<Player>();
        //originalTarget = GameObject.FindGameObjectWithTag("Finish").transform; // assim que começar ele ira logo atras da tag "Player"(Object)
        originalTarget = FindNearestWithTag("Finish").transform;
        targetFollow = originalTarget;
        speed = 3;
        canWalk = true;
        canLeave = false;
        isSatisfied = false;
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        if (targetFollow == null && !isSatisfied) {
            if (originalTarget == null) {
                originalTarget = FindNearestWithTag("Finish").transform;

                if (FindNearestWithTag("Finish").transform != null) {
                    originalTarget = FindNearestWithTag("Finish").transform;
                } else {
                    setSatisfied(true);
                }
            }

            targetFollow = originalTarget;
        }

        if (isSatisfied) {
            targetFollow = FindNearestWithTag("Exit").transform;
        }

        //if (!canWalk)
        //    return;

        if (canWalk && Vector2.Distance(transform.position, targetFollow.position) > 0.1f) {
            transform.position = Vector2.MoveTowards(transform.position, targetFollow.position, speed * Time.deltaTime);//calculando a posição em que o player se move para segui-lo
        }

        //var deltaVector = targetFollow.position - transform.position;
        //var directionDelta = deltaVector.normalized;
        //var newPosition = transform.position + (directionDelta * speed/100);
        //transform.position = newPosition;

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Arma") && !isSatisfied) {
            var playerPosition = player.transform.position;
            var deltaPosition = transform.position - playerPosition;

            var absDelta = deltaPosition.normalized;

            rb.AddForce(absDelta * knockbackForce);

            StartCoroutine(LockWalk(1.5f));
            //StartCoroutine(KnockbackCoroutine());
        }

        if (!canLeave && collision.CompareTag("Trigger")) {
            canLeave = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {    
        if (!isSatisfied && targetFollow != collision.transform && collision.CompareTag("Food")) {
            targetFollow = collision.transform;
        }
    }

    private void OnBecameInvisible() {
        if (canLeave) {
            setSatisfied(true);
            targetFollow = FindNearestWithTag("Exit").transform;
            //TODO
        }
    }

    private GameObject FindNearestWithTag(string tag) {
        GameObject[] objects;
        objects = GameObject.FindGameObjectsWithTag(tag);
        GameObject closest = null;

        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject obj in objects) {
            Vector3 diff = obj.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance) {
                closest = obj;
                distance = curDistance;
            }
        }

        return closest;
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

    public void setSatisfied(bool b) {
        isSatisfied = b;
    }
}
