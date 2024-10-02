using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemCell : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    Image icon;
    public bool isSelectedCell;
    public static bool isHoldingIcon;
    public Slot attachedSlot;  // a slot where the item in this cell is stored
    TextMeshProUGUI amountDisplay;

    void Start()
    {
        icon = transform.GetChild(0).GetComponent<Image>();
        amountDisplay = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        // when drop moving icon onto this cell

        if (eventData.pointerDrag == null)
        {
            return;
        }

        // swap item attached to this cell
        if (ItemInventoryIcon.actuallyDraggingIcon.referenceCell.attachedSlot.item != attachedSlot.item)
        {
            // alter item object
            Slot tmp = ItemInventoryIcon.actuallyDraggingIcon.referenceCell.attachedSlot;
            ItemInventoryIcon.actuallyDraggingIcon.referenceCell.SetAttachedSlot(attachedSlot);
            SetAttachedSlot(tmp);
        }
        else if((attachedSlot.amount + ItemInventoryIcon.actuallyDraggingIcon.referenceCell.attachedSlot.amount) <= attachedSlot.item.maxStackSize)
        {
            // stack same items (if not too many in one cell)
            attachedSlot.amount += ItemInventoryIcon.actuallyDraggingIcon.referenceCell.attachedSlot.amount;
            ItemInventoryIcon.actuallyDraggingIcon.referenceCell.SetAttachedSlot(null);
            Inventory.RemoveFromInventory(ItemInventoryIcon.actuallyDraggingIcon.referenceCell.attachedSlot, ItemInventoryIcon.actuallyDraggingIcon.referenceCell.attachedSlot.amount);
        }
        ItemInventoryIcon.actuallyDraggingIcon.referenceCell.UpdateAmountDisplay();
        UpdateAmountDisplay();

        // if item dropped on this cell was previously on selected one, change item in hand according to item from selected one
        if (ItemInventoryIcon.actuallyDraggingIcon.referenceCell.isSelectedCell)
        {
            Hand.SetItemInHand(QuickAccessInventory.selectedCell.attachedSlot.item);
        }
        isHoldingIcon = false;
    }

    /// <summary>
    /// Set item which is in this cell.
    /// </summary>
    /// <param name="slot"></param>
    public void SetAttachedSlot(Slot slot)
    {
        if (slot != null) 
            attachedSlot = slot;

        // change sprite of the icon (or not if no item)
        try
        {
            icon.sprite = slot.item.uIIcon;
            icon.enabled = true;

            // set item in hand right after drop on selected cell
            if (isSelectedCell)
            {
                Hand.SetItemInHand(attachedSlot.item);
            }
        }
        catch
        {
            icon.sprite = null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!attachedSlot.item || isHoldingIcon)
            return;
        ItemInfo.ToggleItemInfo(true, transform.position, attachedSlot.item.name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!attachedSlot.item)
            return;
        ItemInfo.ToggleItemInfo(false);
    }

    public void UpdateAmountDisplay()
    {
        if (attachedSlot.amount > 1)
            amountDisplay.text = attachedSlot.amount.ToString();
        else
            amountDisplay.text = "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // when already dragging separated item
        if(StackSplit.isDraggingSeparatedIcon)
        {
            if (attachedSlot.item == null)
            {
                Slot newSlot = StackSplit.separatedSlot;
                SetAttachedSlot(newSlot);
                Inventory.inventory.items.Add(newSlot);
                StackSplit.ToggleSplitIcon(false);
                StackSplit.editingCell = null;
                UpdateAmountDisplay();
                return;
            }
        }

        if(eventData.button != PointerEventData.InputButton.Right || attachedSlot.amount <= 1) return;  // block spliting when not possible
        StackSplit.ToggleStackSplitMenu(true, this, attachedSlot.amount);  // split when clicked right click on the cell
    }
}
