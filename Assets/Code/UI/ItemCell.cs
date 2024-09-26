using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemCell : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    Image icon;
    public Item itemInCell;
    public bool isSelectedCell;
    public static bool isHoldingIcon;
    public Slot attachedSlot;  // a slot where the item in this cell is stored

    void Start()
    {
        icon = transform.GetChild(0).GetComponent<Image>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        // when drop moving icon onto this cell

        if (eventData.pointerDrag == null)
        {
            return;
        }

        // swap item attached to this cell
        if (ItemInventoryIcon.actuallyDraggingIcon.referenceCell.itemInCell != itemInCell)
        {
            // alter item object
            Item tmp = ItemInventoryIcon.actuallyDraggingIcon.referenceCell.itemInCell;
            ItemInventoryIcon.actuallyDraggingIcon.referenceCell.SetItemInCell(itemInCell);
            SetItemInCell(tmp);
        }
        else if((attachedSlot.amount + ItemInventoryIcon.actuallyDraggingIcon.referenceCell.attachedSlot.amount) <= attachedSlot.item.maxStackSize)
        {
            // stack same items (if not too many in one cell)
            print("takie same przedmioty");
            attachedSlot.amount += ItemInventoryIcon.actuallyDraggingIcon.referenceCell.attachedSlot.amount;
            ItemInventoryIcon.actuallyDraggingIcon.referenceCell.SetItemInCell(null);
            Inventory.RemoveFromInventory(ItemInventoryIcon.actuallyDraggingIcon.referenceCell.attachedSlot, ItemInventoryIcon.actuallyDraggingIcon.referenceCell, ItemInventoryIcon.actuallyDraggingIcon.referenceCell.attachedSlot.amount);
        }

        // if item dropped on this cell was previously on selected one, change item in hand according to item from selected one
        if (ItemInventoryIcon.actuallyDraggingIcon.referenceCell.isSelectedCell)
        {
            Hand.SetItemInHand(QuickAccessInventory.selectedCell.itemInCell);
        }
        isHoldingIcon = false;
    }

    /// <summary>
    /// Set item which is in this cell.
    /// </summary>
    /// <param name="item"></param>
    public void SetItemInCell(Item item)
    {
        itemInCell = item;

        // change sprite of the icon (or not if no item)
        try
        {
            icon.sprite = item.uIIcon;
            icon.enabled = true;

            // set item in hand right after drop on selected cell
            if (isSelectedCell)
            {
                Hand.SetItemInHand(itemInCell);
            }
        }
        catch
        {
            icon.sprite = null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!itemInCell || isHoldingIcon)
            return;
        ItemInfo.ToggleItemInfo(true, transform.position, itemInCell.name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!itemInCell)
            return;
        ItemInfo.ToggleItemInfo(false);
    }
}
