using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLane : MonoBehaviour {

    private void FixedUpdate() {
        if (gameObject.tag == "Food" || gameObject.tag == "Cleaning") {
            gameObject.transform.Rotate(0, 8f, 0);
        }
    }

    public void PositionLane() {
        int randomLane = Random.Range(-1, 2);
        transform.position = new Vector3(randomLane, transform.position.y, transform.position.z);
    }
}
