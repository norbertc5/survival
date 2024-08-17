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
    List<Image> icons = new List<Image>();

    private void Start()
    {
        inventory = this;

        inventoryToggleAction.action.started += (InputAction.CallbackContext obj) =>
        {
            isInventoryOpen = !isInventoryOpen;
            inventoryContainer.SetActive(isInventoryOpen);
            Player.Freeze(isInventoryOpen);
        };

        for (int i = 0; i < inventoryCapacity; i++)
        {
            icons.Add(cellsContainer.transform.GetChild(i).GetChild(0).GetComponent<Image>());
            //icons[i].color = Color.red;
        }
    }

    public static void AddToInventory(Item item)
    {
        if (inventory.items.Count >= inventory.inventoryCapacity)
        {
            print("return");
            return;
        }

        //inventory.items.Add(item, 1);
        inventory.items.Add(item);
        //inventory.inventoryCapacity = inventory.items.Count;
        print(item.name);
        Image iconImg = inventory.GetFirstFreeCellIcon();
        iconImg.sprite = item.uIIcon;
        iconImg.enabled = true;
    }

    Image GetFirstFreeCellIcon()
    {
        for (int i = 0; i < inventoryCapacity; i++)
        {
            Image icon = icons[i];
            if(icon.sprite == null)
            {
                return icon;
            }
        }
        return null;
    }
}
