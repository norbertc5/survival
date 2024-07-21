using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour, IDamageable
{
    public void GetDamage()
    {
        print($"got damage");
    }
}
