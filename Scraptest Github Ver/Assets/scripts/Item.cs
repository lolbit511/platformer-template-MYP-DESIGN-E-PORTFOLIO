using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject
{
    [Header("STATISTICS")]
    
    //public Vector2Int range = new Vector2Int(5, 4);
    public int health;
    public int defense;
    public int attack;
    public int energy;
    public int wisdom;

    [Header("MODIFIERS")]
    //public modifyType type;
    public bool top;
    public bool bottom;
    public bool left;
    public bool right;
    public bool topLeft;
    public bool topRight;
    public bool bottomLeft;
    public bool bottomRight;
    public bool fullboard;

    [Header("Only UI")]
    public bool stackable = true;

    [Header("apperance")]
    public Sprite image;
}

public enum modifyType
{
    top,
    skill
}