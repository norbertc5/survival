using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StackSplit : MonoBehaviour
{
    static StackSplit instance;
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI splitText;
    [SerializeField] SplitItemIcon separatedSlotIcon;
    public static bool isDraggingSeparatedIcon;
    int separateAmount;
    public static ItemCell editingCell;
    public static Slot separatedSlot;

    private void Start()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    public static void ToggleStackSplitMenu(bool value, ItemCell cell = null, int stackSize = 0)
    {
        instance.gameObject.SetActive(value);

        editingCell = cell;
        if (cell == null)
            return;

        instance.transform.position = cell.transform.position;
        instance.slider.maxValue = stackSize - 1;
        if(instance.slider.maxValue > 0)
            instance.slider.SetValueWithoutNotify(stackSize / 2);
        instance.Slider_OnChange();
    }

    public void Slider_OnChange()
    {
        splitText.text = $"{slider.value} / {slider.maxValue}";
    }

    public void Split()
    {
        // separates new slot from existing one
        // separated slot has item like editing slot and amount from slider
        // separated slot is represended by special icon - separatedSlotIcon

        separateAmount = (int)slider.value;
        if (separateAmount <= 0) return;

        editingCell.attachedSlot.amount -= separateAmount;
        separatedSlot = new Slot(editingCell.attachedSlot.item, separateAmount);
        separatedSlotIcon.GetComponent<Image>().sprite = editingCell.attachedSlot.item.uIIcon;
        editingCell.UpdateAmountDisplay();
        ToggleSplitIcon(true);
        gameObject.SetActive(false);
    }

    public static void ToggleSplitIcon(bool value)
    {
        isDraggingSeparatedIcon = value;
        instance.separatedSlotIcon.gameObject.SetActive(value);
    }
}
