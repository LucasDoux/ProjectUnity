using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OrangeFood : MonoBehaviour
{

    public double timer = 0.0;
    public float Depletion = 10;
    public Transform barTransform;
    public GameObject radius;

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(radius, transform.position, Quaternion.identity);
        //StartCoroutine(Despawn(3));
    }

    // Update is called once per frame
    void Update()
    {
        //timer += Time.deltaTime;

        //if (timer >= 3) {
        //    Destroy(this.gameObject);
        //}
    }

    private void OnCollisionStay2D(Collision2D collision) {
        Debug.Log("a");
        if (collision.gameObject.CompareTag("Cat")) {
            Debug.Log($"Depletion = {Depletion}");
            Depletion--;
            barTransform.localScale = new Vector3(0.5f * (Depletion / 100), 0.04f, 1);
            if (Depletion <= 0) {
                Destroy(gameObject);
            }
        } else {
            Debug.Log(collision.gameObject.tag);
        }
    }

    IEnumerator Despawn(int seconds) {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
