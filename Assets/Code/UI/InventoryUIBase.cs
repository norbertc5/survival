using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIBase : MonoBehaviour
{
    [SerializeField] protected int inventoryCapacity = 3;
    protected List<ItemCell> cells = new List<ItemCell>();
    public Transform cellsContainer;

    protected virtual void Start()
    {
        for (int i = 0; i < cellsContainer.childCount; i++)
        {
            cells.Add(cellsContainer.transform.GetChild(i).GetComponent<ItemCell>());
        }
    }
}
