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
    List<Item> items = new List<Item>(2);
    [SerializeField] AnimationClip hideShowItemAnimation;

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
                Player.ActionWithStamina(() => animator.CrossFade(itemInHand.useAnimation.name, 0), itemInHand.staminaDemand);
                canPerformClickAction = false;
                ActionOnTime.Create(() => { canPerformClickAction = true; }, itemInHand.useAnimation.length);
            }
        };

        // get all items and create model of handable ones in the hand and hide it
        items = Resources.LoadAll<Item>("Items").ToList();
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].isHandable)
            {
                var newItem = Instantiate(items[i].model, transform);
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

    public void InvokeEatBenefit()
    {
        print("You have eaten sth.");
        Inventory.RemoveFromInventory(itemInHand, QuickAccessInventory.selectedCell);
    }

    Item oldItem;
    public static void SetItemInHand(Item newItem, bool playHideAnim = true)
    {
        if (hand.itemInHand) hand.oldItem = hand.itemInHand;
        if (hand.oldItem && playHideAnim) hand.animator.CrossFade("hide", 0);  // if player has old item in hand, play hide anim

        hand.itemInHand = newItem;
        ActionOnTime.Stop("SetItemInHand");  // repairs issue with no model while item was in hand

        if (newItem != null)
        {
            // if we have item in our hand, wait till hide anim ends and then play show one
            // but if no item in hand, no hide anim, so don't need to wait :)
            float t = hand.oldItem ? hand.hideShowItemAnimation.length : 0;
            ActionOnTime.Create(() =>
            {
                hand.animator.CrossFade("show", 0);

                // changing the model
                // I konw that this is bad way to do that but I don't have anything better now
                for (int i = 0; i < hand.transform.childCount; i++)
                    hand.transform.GetChild(i).gameObject.SetActive(false);
                hand.transform.Find(newItem.name).gameObject.SetActive(true);
            }, t);
        }
        else
        {
            ActionOnTime.Create(() =>
            {
                if(hand.oldItem) hand.transform.Find(hand.oldItem.name).gameObject.SetActive(false);
            }, hand.hideShowItemAnimation.length, "SetItemInHand");
        }
    }
}
