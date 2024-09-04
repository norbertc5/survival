using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;

public class QuickAccessInventory : InventoryUIBase
{
    [SerializeField] InputActionReference changeItemAction;
    [SerializeField] InputActionReference changeItemKeysAction;
    [SerializeField] Transform frame;
    int actualCellId;
    public static ItemCell selectedCell;
    int keyInput = -1;  // -1 = no input

    protected override void Start()
    {
        base.Start();
        selectedCell = cells[0];
        selectedCell.isSelectedCell = true;

    }

    void Update()
    {
        #region Handle scroll item changing
        float scroll = -changeItemAction.action.ReadValue<Vector2>().y;
        int scrollDir = Math.Sign(scroll);

        // if player's scrolling and frame in boudns
        if (scroll != 0 && actualCellId + scrollDir >= 0 && actualCellId + scrollDir < inventoryCapacity)
        {
            actualCellId += scrollDir;

            selectedCell.isSelectedCell = false;
            selectedCell = cells[actualCellId];
            selectedCell.isSelectedCell = true;

            Hand.SetItemInHand(cells[actualCellId].itemInCell);
        }
        #endregion

        #region Handle keys changing
        changeItemKeysAction.action.performed += ctx => keyInput = (int)ctx.ReadValue<float>();  // read input

        if(keyInput != -1 && actualCellId != keyInput-1)
        {
            actualCellId = keyInput-1;

            selectedCell.isSelectedCell = false;
            selectedCell = cells[actualCellId];
            selectedCell.isSelectedCell = true;

            Hand.SetItemInHand(cells[keyInput-1].itemInCell);
            keyInput = -1;
        }
        #endregion

        // update frame pos
        frame.position = cells[actualCellId].transform.position;
    }
}
