using norbertcUtilities.ActionOnTime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory inventory;
    [SerializeField] InputActionReference inventoryToggleAction;
    [SerializeField] GameObject inventoryContainer;
    [SerializeField] GameObject cellsContainer;

    //public Dictionary<Item, int> items = new Dictionary<Item, int>();
    public List<Item> items = new List<Item>();
    bool isInventoryOpen;
    [SerializeField] int inventoryCapacity = 9;
    List<ItemCell> cells = new List<ItemCell>();

    private void Start()
    {
        inventory = this;

        inventoryToggleAction.action.started += (InputAction.CallbackContext obj) =>
        {
            isInventoryOpen = !isInventoryOpen;
            inventoryContainer.SetActive(isInventoryOpen);
            Player.Freeze(isInventoryOpen);
        };

        // get ui inventory cells
        for (int i = 0; i < inventoryCapacity; i++)
        {
            cells.Add(cellsContainer.transform.GetChild(i).GetComponent<ItemCell>());
        }

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
        freeCell.SetItem(item);
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
