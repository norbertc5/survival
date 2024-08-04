using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ItemsHealthBar : UIBar
{
    [SerializeField] Transform player;

    void Update()
    {
        transform.LookAt(player);
    }

    public override void SetBar(float fill, Vector3 pos)
    {
        base.SetBar(fill);
        Show();
        transform.position = pos + Vector3.up;
    }
}
