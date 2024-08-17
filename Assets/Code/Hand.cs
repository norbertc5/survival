using norbertcUtilities.ActionOnTime;
using System;
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

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        clickAction.action.started += (InputAction.CallbackContext obj) =>
        {
            if(canPerformClickAction && !Player.isPlayerFreeze)
            {
                Player.ActionWithStamina(() => animator.CrossFade(itemInHand.attackAnimation.name, 0), itemInHand.staminaDemand);
                canPerformClickAction = false;
                ActionOnTime.Create(() => { canPerformClickAction = true; }, itemInHand.attackAnimation.length);
            }
        };
    }

    public void InvokeAttack()
    {
        // attack is invoking via animation 
        attack?.Invoke(itemInHand.damage);
    }
}
