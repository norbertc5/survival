using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [SerializeField] float sensitivity = 100;
    [SerializeField] Transform playerBody;
    [SerializeField] InputActionReference lookAction;
    float xRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Vector2 look = lookAction.action.ReadValue<Vector2>();
        float mouseX = look.x * sensitivity * Time.deltaTime;
        float mouseY = look.y * sensitivity * Time.deltaTime;

        xRotation -= mouseY; // get rotation of object, not mouse move
        xRotation = Mathf.Clamp(xRotation, -90, 90);        

        playerBody.Rotate(new Vector2(0, mouseX));
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }
}
