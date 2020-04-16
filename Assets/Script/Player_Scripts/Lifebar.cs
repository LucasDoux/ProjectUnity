using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Lifebar : MonoBehaviour {

    #region Variables

    public RectTransform LayoutContent;
    public GameObject LifeBarPrefab;
    public List<GameObject> Lifebars;

    #endregion

    #region Functions

    public void UpdateLife(int life) {
        if (life == Lifebars.Count)
            return;

        while (Lifebars.Count < life) {
            var newLifebar = Instantiate(LifeBarPrefab, LayoutContent);
            Lifebars.Add(newLifebar);
        }

        while (Lifebars.Count > life) {
            var lifebar = Lifebars.First();
            Lifebars.Remove(lifebar);
            Destroy(lifebar);
        }
    }

    public void DecreaseLife() {
        UpdateLife(GetLife() - 1);
    }

    public int GetLife() {
        return Lifebars.Count;
    }

    #endregion
}
