using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radius : MonoBehaviour
{
    public double timer = 0.0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 3) {
            Destroy(this.gameObject);
        }
    }
}
