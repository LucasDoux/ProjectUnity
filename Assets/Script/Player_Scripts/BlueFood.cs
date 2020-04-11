using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueFood : OrangeFood {
    protected override void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Cat")) {
            if (barTransform.gameObject.GetComponent<SpriteRenderer>().enabled == false) {
                barTransform.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }

            Depletion--;
            barTransform.localScale = new Vector3(0.5f * (Depletion / OriginalDepletion), 0.04f, 1);
            if (Depletion == 0) {
                var cat = collision.gameObject.GetComponent<EnemyFollow>();
                cat.State = EnemyFollow.CatState.Leaving;
                Destroy(gameObject);
            }
        } else {
            Debug.Log(collision.gameObject.tag);
        }
    }
}
