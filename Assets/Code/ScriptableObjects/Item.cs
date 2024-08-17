using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Item")]
public class Item : ScriptableObject
{
    new public string name;
    public GameObject model;
    public AnimationClip attackAnimation;
    public Sprite uIIcon;
    public int staminaDemand = 1;
    public int damage = 10;
}
