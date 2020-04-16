using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodHumans : MonoBehaviour {

    #region Variables

    public float Depletion = 220;
    private float OriginalDepletion;
    public Transform barTransform;

    #endregion

    #region Unity Events

    void Start() {
        OriginalDepletion = Depletion;
    }
    
    void Update() {
        
    }

    private void OnDestroy() {
        GameController.Instance.Food.Remove(gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Cat")) {
            if (barTransform.gameObject.GetComponent<SpriteRenderer>().enabled == false) {
                barTransform.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }

            //Debug.Log($"Depletion = {Depletion}");
            Depletion--;
            barTransform.localScale = new Vector3(0.5f * (Depletion / OriginalDepletion), 0.04f, 1);
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

    #endregion
}
