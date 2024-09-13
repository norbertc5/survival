using norbertcUtilities.ActionOnTime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : InventoryUIBase
{
    public static Inventory inventory;
    [SerializeField] InputActionReference inventoryToggleAction;
    [SerializeField] GameObject inventoryContainer;
    [SerializeField] Transform dropPoint;
    [SerializeField] float dropAreaRadius;

    [SerializeField] public List<Item> items = new List<Item>();
    bool isInventoryOpen;
    public ItemOnGround dropItem;

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

    public static void RemoveFromInventory(Item item, ItemCell itemCell)
    {
        itemCell.SetItemInCell(null);

        if(QuickAccessInventory.selectedCell == itemCell)
            Hand.SetItemInHand(null);
        inventory.items.Remove(item);
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

    [SerializeField] LayerMask dropCheckMask;
    public static void Drop(Item toDrop, ItemCell itemCell)
    {
        ItemOnGround droppedInstance = Instantiate(inventory.dropItem);
        droppedInstance.item = toDrop;
        ItemCell.isHoldingIcon = false;
        float yPos = inventory.transform.position.y - .5f;  // y pos for dropped instance

        // if we stand too close to obstacle, dropped item would appear into one
        // so to prevent that, we check area around player to check if we stand too close to obstacle
        // and if so, we place the dropped item in arranged pos
        if (Physics.CheckSphere(inventory.transform.position, 1, inventory.dropCheckMask))
        {
            droppedInstance.transform.position = new Vector3(inventory.transform.position.x, yPos, inventory.transform.position.z);
            droppedInstance.transform.position += droppedInstance.transform.forward / 5;
        }
        else
        {
            // place dropped in random pos but close to dropPoint
            Vector2 circle = (Vector3)Random.insideUnitCircle * inventory.dropAreaRadius;
            Vector3 randomPos = new Vector3(inventory.dropPoint.position.x + circle.x, yPos, circle.y + inventory.dropPoint.position.z);
            droppedInstance.transform.position = randomPos;
        }
        RemoveFromInventory(toDrop, itemCell);
    }
}
