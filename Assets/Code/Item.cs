using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Item")]
public class Item : ScriptableObject
{
    public GameObject model;
    public AnimationClip useAnimation;
}
