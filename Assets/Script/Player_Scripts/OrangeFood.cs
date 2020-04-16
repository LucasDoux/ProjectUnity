using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OrangeFood : MonoBehaviour {

    #region Variables

    public float Depletion = 175;
    public Transform areaTransform;
    protected float OriginalDepletion;
    public Transform barTransform;
    public static List<OrangeFood> Food = new List<OrangeFood>();

    public float Radius => areaTransform.localScale.x/2;

    #endregion

    #region Unity Events
    void Start() {
        OriginalDepletion = Depletion;
        Food.Add(this);
    }

    private void OnDestroy() {
        Food.Remove(this);
    }

    protected virtual void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Cat")) {
            if (barTransform.gameObject.GetComponent<SpriteRenderer>().enabled == false) {
                barTransform.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }

            //Debug.Log($"Depletion = {Depletion}");
            Depletion--;
            barTransform.localScale = new Vector3(0.5f * (Depletion / OriginalDepletion), 0.04f, 1);
            if (Depletion == 0) {
                Destroy(gameObject);
            }
        } else {
            Debug.Log(collision.gameObject.tag);
        }
    }

    #endregion
}
