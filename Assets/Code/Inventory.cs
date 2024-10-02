using norbertcUtilities.ActionOnTime;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class Inventory : InventoryUIBase
{
    public static Inventory inventory;
    [SerializeField] InputActionReference inventoryToggleAction;
    [SerializeField] GameObject inventoryContainer;
    [SerializeField] Transform dropPoint;
    [SerializeField] float dropAreaRadius;

    [SerializeField] public List<Slot> items = new List<Slot>();
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
            StackSplit.ToggleStackSplitMenu(false);
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

    public static void AddToInventory(Item item, int amount)
    {
        // if no place in inventory
        if (inventory.items.Count >= inventory.inventoryCapacity)
        {
            return;
        }

        // if we have not full stack of this item in the inventory
        Slot slotWithItem = inventory.GetSlotWithItem(item, amount);
        if (slotWithItem != null)
        {
            slotWithItem.amount += amount;
            if(inventory.GetCellWithSlot(slotWithItem)) inventory.GetCellWithSlot(slotWithItem).UpdateAmountDisplay();
        }
        else  // if we don't have this item in the inventory so far
        {
            Slot newSlot = new Slot(item, amount);
            inventory.items.Add(newSlot);
            ItemCell freeCell = inventory.GetFirstFreeCell();
            freeCell.attachedSlot = newSlot;
            freeCell.SetAttachedSlot(newSlot);
            freeCell.UpdateAmountDisplay();
        }
    }

    public static void RemoveFromInventory(Slot slot, int amount)
    {
        if(amount > slot.amount)
        {
            print("You're trying to remove more than you have.");
            return;
        }

        // decrease amount of the item and if we have 0 pieces, remove it completly
        slot.amount -= amount;
        ItemCell cell = inventory.GetCellWithSlot(slot);
        cell.UpdateAmountDisplay();

        if (slot.amount == 0)
        {
            cell.attachedSlot.item = null;
            cell.SetAttachedSlot(null);

            if (QuickAccessInventory.selectedCell == cell)
                Hand.SetItemInHand(null);
            inventory.items.Remove(slot);
        }
    }

    ItemCell GetFirstFreeCell()
    {
        ItemCell[] cells = inventory.cells.Where(t => t.attachedSlot.item == null).Select(t => t).ToArray();
        if (cells.Length > 0)
            return cells[0];
        else return null;
    }

    Slot GetSlotWithItem(Item item, int amountToAdd)
    {
        Slot[] slot = inventory.items.Where(t => t.item == item && (t.amount + amountToAdd) <= item.maxStackSize).Select(t => t).ToArray();
        if(slot.Length > 0)
            return slot[0];
        else
            return null;
    }

    ItemCell GetCellWithSlot(Slot slot)
    {
        ItemCell[] cells = inventory.cells.Where(t => t.attachedSlot == slot).Select(t => t).ToArray();
        if(cells.Length > 0)
            return cells[0];
        else return null;
    }

    [SerializeField] LayerMask dropCheckMask;
    public static void Drop(Slot slot)
    {
        ItemOnGround droppedInstance = Instantiate(inventory.dropItem);
        droppedInstance.item = slot.item;
        droppedInstance.amount = slot.amount;
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
            Vector2 circle = (Vector3)UnityEngine.Random.insideUnitCircle * inventory.dropAreaRadius;
            Vector3 randomPos = new Vector3(inventory.dropPoint.position.x + circle.x, yPos, circle.y + inventory.dropPoint.position.z);
            droppedInstance.transform.position = randomPos;
        }
        if(inventory.items.Contains(slot))
            RemoveFromInventory(slot, slot.amount);
    }
}

[Serializable]
public class Slot
{
    // in slot we hold item and amount
    // we store slots in inventory
    public Item item;
    public int amount;

    public Slot(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }
}
