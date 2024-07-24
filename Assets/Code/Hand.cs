using norbertcUtilities.ActionOnTime;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hand : MonoBehaviour
{
    public Action<int> attack;

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
            animator.CrossFade(itemInHand.useAnimation.name, 0);
        };
    }

    public void InvokeAttack()
    {
        // attack is invoking via animation 
        attack?.Invoke(10);
    }
}
