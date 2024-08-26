using norbertcUtilities.ActionOnTime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : InventoryUIBase
{
    public static Inventory inventory;
    [SerializeField] InputActionReference inventoryToggleAction;
    [SerializeField] GameObject inventoryContainer;

    public List<Item> items = new List<Item>();
    bool isInventoryOpen;

    protected override void Start()
    {
        inventory = this;

        inventoryToggleAction.action.started += (InputAction.CallbackContext obj) =>
        {
            isInventoryOpen = !isInventoryOpen;
            inventoryContainer.SetActive(isInventoryOpen);
            Player.Freeze(isInventoryOpen);
        };

        #region Set the cells in a list
        QuickAccessInventory quickAccessCellsContainer = FindObjectOfType<QuickAccessInventory>();

        // firstly get cells from quick access to make picked up items go there first
        for (int i = 0; i < quickAccessCellsContainer.cellsContainer.childCount; i++)
        {
            cells.Add(quickAccessCellsContainer.cellsContainer.GetChild(i).GetComponent<ItemCell>());
        }
        base.Start();  // then normally get your own cells
        #endregion

        // lets to set up ui inventory elements
        inventoryContainer.SetActive(true);
        ActionOnTime.Create(() => { inventoryContainer.SetActive(false); }, .01f);
    }

    public static void AddToInventory(Item item)
    {
        if (inventory.items.Count >= inventory.inventoryCapacity)
        {
            return;
        }

        inventory.items.Add(item);
        ItemCell freeCell = inventory.GetFirstFreeCell();
        freeCell.SetItemInCell(item);
    }

    ItemCell GetFirstFreeCell()
    {
        for (int i = 0; i < inventoryCapacity; i++)
        {
            ItemCell cell = cells[i];
            if(cell.itemInCell == null)
            {
                return cell;
            }
        }
        return null;
    }
}
