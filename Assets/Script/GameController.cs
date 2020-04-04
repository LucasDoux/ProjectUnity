using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour {
    public GameObject FoodPrefab;
    public BoxCollider2D FoodSpawn;
    public float minDistance;

    public void Awake() {
        var bounds = FoodSpawn.bounds;
        var min = bounds.min;
        var max = bounds.max;

        Vector3 pos1 = Vector3.zero;
        Vector3 pos2 = Vector3.zero;
        bool valid = false;
        
        int maxAttempts = 30;
        int attemps = 0;
        
        while (!valid && attemps <= maxAttempts) {
            attemps++;
            pos1 = new Vector3(Random.Range(min.x,max.x), Random.Range(min.y,max.y));
            pos2 = new Vector3(Random.Range(min.x,max.x), Random.Range(min.y,max.y));

            var distance = Vector3.Distance(pos1, pos2);

            valid = distance >= minDistance;
        }

        Instantiate(FoodPrefab, pos1, Quaternion.identity);
        Instantiate(FoodPrefab, pos2, Quaternion.identity);
    }
}
