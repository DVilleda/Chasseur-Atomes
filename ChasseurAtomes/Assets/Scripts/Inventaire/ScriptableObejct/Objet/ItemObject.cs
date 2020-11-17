using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Atomes,
    Default
}
public abstract class ItemObject : ScriptableObject
{
    public GameObject prefab;
    public Sprite icon;
    public ItemType type;
    public int electrons;
}
