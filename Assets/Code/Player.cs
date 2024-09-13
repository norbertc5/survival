using norbertcUtilities.FirstPersonMovement;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Stamina")]
    [SerializeField] int maxStamina = 10;
    [SerializeField] float timeToRestoreStamina = 2;
    [SerializeField] float staminaRestoringSpeed = 0.5f;
    Coroutine restoreStaminaRoutine;
    public static int Stamina { get; private set; }

    [Header("Interaction")]
    [SerializeField] float interactionDistance = 10;

    public static bool isPlayerFreeze;

    [Header("References")]
    [SerializeField] UIBar staminaBar;
    InteractionItemNameDisplay interactionItemNameDisplay;
    Transform selectedItem;
    [SerializeField] InputActionReference interactionAction;
    FirstPresonMovement firstPresonMovement;
    MouseLook mouseLook;
    public static Player player;

    private void Start()
    {
        Stamina = maxStamina;
        staminaBar.Hide();
        interactionItemNameDisplay = FindObjectOfType<InteractionItemNameDisplay>();
        firstPresonMovement = GetComponent<FirstPresonMovement>();
        mouseLook = FindObjectOfType<MouseLook>();
        player = this;

        interactionAction.action.started += (InputAction.CallbackContext obj) =>
        {
            if (selectedItem == null)
                return;

            Inventory.AddToInventory(selectedItem.GetComponent<ItemOnGround>().item);
            selectedItem.gameObject.SetActive(false);
            selectedItem = null;
        };
    }

    private void FixedUpdate()
    {
        Transform camera = Camera.main.transform;
        Physics.Raycast(camera.position, camera.forward, out RaycastHit hit, interactionDistance);

        if (hit.transform != null && hit.transform.CompareTag("ItemOnGround"))
        {
            interactionItemNameDisplay.ShowAndSet(hit.transform.GetComponent<ItemOnGround>().item.name);
            if(selectedItem) selectedItem.SendMessage("DisableOutline");
            selectedItem = hit.transform;
            selectedItem.SendMessage("EnableOutline");
        }
        else
        {
            if (selectedItem != null) selectedItem.SendMessage("DisableOutline");
            interactionItemNameDisplay.Hide();
            selectedItem = null;
        }
    }

    /// <summary>
    /// Perform action which demands stamina.
    /// </summary>
    /// <param name="action"></param>
    /// <param name="staminaDemand"></param>
    public static void ActionWithStamina(Action action, int staminaDemand)
    {
        if (isPlayerFreeze)
            return;

        // if this action make stamina equals or less than 0, blink the bar
        if ((Stamina - staminaDemand) <= 0 && staminaDemand > 0)
        {
            player.staminaBar.Show();
            player.staminaBar.BackgroundBlink(new Color32(255, 0, 0, 1), 3, .2f);
        }

        // if not enough stamina to perform action, return
        if ((Stamina - staminaDemand) < 0)
            return;

        // perform action
        action();

        if (staminaDemand > 0)
        {
            Stamina -= staminaDemand;
            player.staminaBar.SetBar(UIBar.ValueToBarFill(Stamina, player.maxStamina));
            player.staminaBar.Show();

            // restoring stamina
            if (player.restoreStaminaRoutine != null) player.StopCoroutine(player.restoreStaminaRoutine);
            player.restoreStaminaRoutine = player.StartCoroutine(player.RestoreStamina());
        }
    }

    IEnumerator RestoreStamina()
    {
        yield return new WaitForSeconds(timeToRestoreStamina);
        while(Stamina < maxStamina)
        {
            Stamina++;
            staminaBar.SetBar(UIBar.ValueToBarFill(Stamina, maxStamina));
            yield return new WaitForSeconds(staminaRestoringSpeed);
        }
    }

    public static void Freeze(bool value)
    {
        isPlayerFreeze = value;
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        player.firstPresonMovement.canMove = !value;
        player.mouseLook.enabled = !value;
    }
}
