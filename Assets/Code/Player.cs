using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] int maxStamina = 10;
    [SerializeField] float timeToRestoreStamina = 2;
    [SerializeField] float staminaRestoringSpeed = 0.5f;
    [SerializeField] UIBar staminaBar;
    Coroutine restoreStaminaRoutine;

    public int Stamina { get; private set; }

    private void Start()
    {
        Stamina = maxStamina;
        staminaBar.Hide();
    }

    /// <summary>
    /// Perform action which demands stamina.
    /// </summary>
    /// <param name="action"></param>
    /// <param name="staminaDemand"></param>
    public void ActionWithStamina(Action action, int staminaDemand)
    {
        // if this action make stamina equals or less than 0, blink the bar
        if ((Stamina - staminaDemand) <= 0)
        {
            staminaBar.Show();
            staminaBar.BackgroundBlink(new Color32(255, 0, 0, 1), 3, .2f);
        }

        // if not enough stamina to perform action, return
        if ((Stamina - staminaDemand) < 0)
            return;

        // perform action
        action();
        Stamina -= staminaDemand;
        staminaBar.SetBar(UIBar.ValueToBarFill(Stamina, maxStamina));
        staminaBar.Show();

        // restoring stamina
        if (restoreStaminaRoutine != null) StopCoroutine(restoreStaminaRoutine);
        restoreStaminaRoutine = StartCoroutine(RestoreStamina());
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
}
