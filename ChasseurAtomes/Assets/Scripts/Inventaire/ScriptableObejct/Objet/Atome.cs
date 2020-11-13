using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Nouveau Objet Atome",menuName ="Inventaire/Items/Atome")]
public class Atome : ItemObject
{

    public void Awake()
    {
        type = ItemType.Atomes;
    }
}
