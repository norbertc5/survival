using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsHealthBar : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Image bar;

    void Update()
    {
        transform.LookAt(player);
    }

    public void SetHealthBar(Vector3 pos, float fill)
    {
        transform.position = pos + Vector3.up;
        print(fill);
        bar.fillAmount = fill;
    }
}
