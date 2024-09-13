using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemCell : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    Image icon;
    public Item itemInCell;
    public bool isSelectedCell;
    public static bool isHoldingIcon;

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
        Item tmp = eventData.pointerDrag.GetComponent<ItemInventoryIcon>().referenceCell.itemInCell;
        eventData.pointerDrag.GetComponent<ItemInventoryIcon>().referenceCell.SetItemInCell(itemInCell);
        SetItemInCell(tmp);

        // if item dropped on this cell was previously on selected one, change item in hand according to item from selected one
        if (eventData.pointerDrag.GetComponent<ItemInventoryIcon>().referenceCell.isSelectedCell)
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
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!itemInCell)
            return;
    }
}
