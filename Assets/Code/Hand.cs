using norbertcUtilities.ActionOnTime;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hand : MonoBehaviour
{
    public Action<int> attack;
    bool canPerformClickAction = true;

    [Header("References")]
    [SerializeField] InputActionReference clickAction;
    [SerializeField] Item itemInHand;
    Animator animator;
    public static Hand hand;
    List<Item> items = new List<Item>(1);

    private void Awake()
    {
        animator = GetComponent<Animator>();
        hand = this;
    }

    private void Start()
    {
        clickAction.action.started += (InputAction.CallbackContext obj) =>
        {
            if(canPerformClickAction && !Player.isPlayerFreeze && itemInHand)
            {
                Player.ActionWithStamina(() => animator.CrossFade(itemInHand.attackAnimation.name, 0), itemInHand.staminaDemand);
                canPerformClickAction = false;
                ActionOnTime.Create(() => { canPerformClickAction = true; }, itemInHand.attackAnimation.length);
            }
        };

        // get all items and create model of handable ones in the hand and hide it
        items = Resources.LoadAll<Item>("Items").ToList();
        for (int i = 0; i < items.Count; i++)
        {
            if (items[0].isHandable)
            {
                var newItem = Instantiate(items[0].model, transform);
                newItem.transform.localPosition = Vector3.zero;
                newItem.name = items[i].name;
                newItem.SetActive(false);
            }
        }
    }

    public void InvokeAttack()
    {
        // attack is invoking via animation 
        attack?.Invoke(itemInHand.damage);
    }

    public static void UpdateItemInHand(Item newItem)
    {
        hand.itemInHand = newItem;

        if (newItem != null)
            hand.transform.Find(newItem.name).gameObject.SetActive(true);
        else
        {
            for (int i = 0; i < hand.transform.childCount; i++)
            {
                hand.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
