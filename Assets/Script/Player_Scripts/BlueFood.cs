using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueFood : MonoBehaviour {

    public float Depletion = 100;
    private float OriginalDepletion;
    public Transform barTransform;

    // Start is called before the first frame update
    void Start() {
        OriginalDepletion = Depletion;
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Cat")) {
            if (barTransform.gameObject.GetComponent<SpriteRenderer>().enabled == false) {
                barTransform.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }

            Depletion--;
            barTransform.localScale = new Vector3(0.5f * (Depletion / OriginalDepletion), 0.04f, 1);
            if (Depletion == 0) {
                var cat = collision.gameObject.GetComponent<EnemyFollow>();
                Destroy(gameObject);
                cat.setSatisfied(true);
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
