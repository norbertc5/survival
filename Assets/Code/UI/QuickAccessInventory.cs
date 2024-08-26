using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;

public class QuickAccessInventory : InventoryUIBase
{
    [SerializeField] InputActionReference changeItemAction;
    [SerializeField] Transform frame;
    int actualCell;
    public static ItemCell selectedCell;

    protected override void Start()
    {
        base.Start();
        selectedCell = cells[0];
        selectedCell.isSelectedCell = true;
    }

    void Update()
    {
        float scroll = -changeItemAction.action.ReadValue<Vector2>().y;
        int scrollDir = Math.Sign(scroll);

        // if player's scrolling and frame in boudns
        if (scroll != 0 && actualCell + scrollDir >= 0 && actualCell + scrollDir < inventoryCapacity)
        {
            actualCell += scrollDir;
            frame.position = cells[actualCell].transform.position;

            selectedCell.isSelectedCell = false;
            selectedCell = cells[actualCell];
            selectedCell.isSelectedCell = true;

            Hand.SetItemInHand(cells[actualCell].itemInCell);
        }
    }
}
