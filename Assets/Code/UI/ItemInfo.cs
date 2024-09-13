using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    static ItemInfo itemInfo;
    TextMeshProUGUI display;

    private void Start()
    {
        itemInfo = this;
        display = GetComponentInChildren<TextMeshProUGUI>();
        gameObject.SetActive(false); // it's toggle on in inspector to let getComponent work
    }

    /// <summary>
    /// Switches item information. You can set it's pos and text inside.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="pos"></param>
    /// <param name="itemName"></param>
    public static void ToggleItemInfo(bool value, Nullable<Vector3> pos = null, string itemName = "")
    {
        itemInfo.gameObject.SetActive(value);

        if (pos != null)
        {
            itemInfo.transform.position = (Vector3)pos;
            itemInfo.display.text = itemName;
        }
    }
}
