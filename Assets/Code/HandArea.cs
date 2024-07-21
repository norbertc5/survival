using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandArea : MonoBehaviour
{
    Hand handScript;

    private void Start()
    {
        handScript = FindObjectOfType<Hand>();
    }

    // whenever something damageable is in the area, it subscribes the action and gets damage when player invokes it
    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if(damageable != null)
        {
            handScript.interaction += damageable.GetDamage;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            handScript.interaction -= damageable.GetDamage;
        }
    }
}
