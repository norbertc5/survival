using System;
using System.Collections;
using System.Collections.Generic;
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
            attack?.Invoke(10);
            animator.CrossFade(itemInHand.useAnimation.name, 0);
        };
    }
}
