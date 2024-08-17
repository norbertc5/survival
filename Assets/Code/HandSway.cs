using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandSway : MonoBehaviour
{
    [SerializeField] InputActionReference lookAction;
    [SerializeField] float smooth = 5;
    [SerializeField] float swayMultiplayer = 5;

    void Update()
    {
        Vector2 look = Vector2.zero;

        if(!Player.isPlayerFreeze)
            look = lookAction.action.ReadValue<Vector2>() * swayMultiplayer;

        Quaternion rotationX = Quaternion.AngleAxis(-look.y, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(look.x, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, (1 /smooth) * Time.deltaTime);
    }
}
