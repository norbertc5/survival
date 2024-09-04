using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName="Item")]
public class Item : ScriptableObject
{
    new public string name;
    public GameObject model;
    public Type type;
    public AnimationClip useAnimation;
    public Sprite uIIcon;
    public int staminaDemand = 1;
    public int damage = 10;
    public bool isHandable;

    public enum Type { weapon, food};
}

[CustomEditor(typeof(Item))]
public class ItemEditor : Editor
{
    // tutorial: https://www.youtube.com/watch?v=H3pCcKnBRHw
    public override void OnInspectorGUI()
    {
        var script = (Item)target;

        script.name = EditorGUILayout.TextField("Name", script.name);
        script.model = EditorGUILayout.ObjectField("Model", script.model, typeof(GameObject), true) as GameObject;
        script.type = (Item.Type)EditorGUILayout.EnumFlagsField("Type", script.type);
        script.useAnimation = EditorGUILayout.ObjectField("Use Animation", script.useAnimation, typeof(AnimationClip), false) as AnimationClip;
        script.uIIcon = EditorGUILayout.ObjectField("UI Icon", script.uIIcon, typeof(Sprite), false) as Sprite;
        script.isHandable = EditorGUILayout.Toggle("Is Handable", script.isHandable);

        if(script.type == Item.Type.weapon)
        {
            script.staminaDemand = EditorGUILayout.IntField("Stamina Demadnd", script.staminaDemand);
            script.damage = EditorGUILayout.IntField("Damage", script.damage);
        }
    }
}
