using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Lifebar : MonoBehaviour
{
    public RectTransform LayoutContent;
    public GameObject LifeBarPrefab;
    public List<GameObject> Lifebars = new List<GameObject>();

    public void UpdateLife(int life)
    {
        if (life == Lifebars.Count)
            return;

        while (Lifebars.Count < life)
        {
            var newLifebar = Instantiate(LifeBarPrefab, LayoutContent);
            Lifebars.Add(newLifebar);
        }

        while (Lifebars.Count > life)
        {
            var lifebar = Lifebars.Last();
            Lifebars.Remove(lifebar);
            Destroy(lifebar);
        }
    }
}
