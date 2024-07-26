using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] int maxStamina = 10;
    [SerializeField] UIBar staminaBar;

    int stamina;

    private void Start()
    {
        stamina = maxStamina;
    }

    /// <summary>
    /// Perform action which demands stamina.
    /// </summary>
    /// <param name="action"></param>
    /// <param name="staminaDemand"></param>
    public void ActionWithStamina(Action action, int staminaDemand)
    {
        staminaBar.SetBar(UIBar.ValueToBarFill(stamina, maxStamina));

        if ((stamina - staminaDemand) < 0)
        {
            staminaBar.BackgroundBlink(new Color32(255, 0, 0, 1), 3, .2f);
            return;
        }

        action();
        stamina -= staminaDemand;
    }
}
