using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeAtome 
{
    Metal,
    NonMetal,
    GasRare
}
[CreateAssetMenu(fileName ="Nouveau Objet Atome",menuName ="Inventaire/Items/Atome")]
public class Atome : ItemObject
{
    public TypeAtome TypeAtome;
    public void Awake()
    {
        type = ItemType.Atomes;
    }
}
