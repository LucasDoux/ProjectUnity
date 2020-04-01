using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodHumans : MonoBehaviour {

    public float Depletion = 200;
    public Transform barTransform;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Cat")) {
            Debug.Log($"Depletion = {Depletion}");
            Depletion--;
            barTransform.localScale = new Vector3(0.5f * (Depletion / 200), 0.04f, 1);
            if (Depletion == 0) {
                Destroy(gameObject);
                GameObject.FindObjectOfType<Lifebar>().DecreaseLife();
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
