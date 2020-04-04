using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arma : MonoBehaviour {
    public LayerMask EnemyLayer;
    public Collider2D ArmaAreaCollider;
    private int counter;

    public void FixedUpdate() {
        counter++;
        if (counter >= 10) {
            counter = 0;

            //ArmaAreaCollider.cont
        }
    }
}
