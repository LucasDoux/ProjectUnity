using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OrangeFood : MonoBehaviour
{

    public double timer = 0.0;
    public GameObject radius;

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(radius, transform.position, Quaternion.identity);
        StartCoroutine(Despawn(3));
    }

    // Update is called once per frame
    void Update()
    {
        //timer += Time.deltaTime;

        //if (timer >= 3) {
        //    Destroy(this.gameObject);
        //}
    }

    

    IEnumerator Despawn(int seconds) {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
