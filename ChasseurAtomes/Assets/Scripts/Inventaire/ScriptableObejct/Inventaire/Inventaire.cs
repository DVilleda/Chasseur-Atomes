using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[CreateAssetMenu(fileName ="Nouvel Inventaire",menuName ="Inventory System/Inventaire")]
public class Inventaire : ScriptableObject, ISerializationCallbackReceiver
{
    public string savePath;
    public ItemDatabaseObject itemDatabaseObject;
    public List<InventorySlot> Conteneur = new List<InventorySlot>();

    public void AjouterItem(ItemObject _item, int _quantite) 
    {
        for (int i = 0; i < Conteneur.Count(); i++) 
        {
            if (Conteneur[i].item == _item)
            {
                Conteneur[i].AddAmount(_quantite);
                return;
            }
        }
            Conteneur.Add(new InventorySlot(itemDatabaseObject.GetId[_item], _item, _quantite));
    }

    public bool creerUnLien(int TailleInv,string typeLien) 
    {
        if (Conteneur.Count<1) 
        {
            return false;
        }
        int TotalElectrons = 0;
        if (typeLien.Equals("Ionique"))
        {
            for (int i=0;i<TailleInv;i++) 
            {
                TotalElectrons += (Conteneur[i].quantite * Conteneur[i].item.electrons);
            }
            if (TotalElectrons % 8 == 0)
            {
                return true;
            }
        }
        else if (typeLien.Equals("Covalente"))
        {
            for (int i = 0; i < TailleInv; i++)
            {
                TotalElectrons += (Conteneur[i].quantite * Conteneur[i].item.electrons) % 2;
            }
            if (TotalElectrons%2==0)
            {
                return true;
            }
        }
        return false;
    }

    public void Save()
    {
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file, saveData);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();
        }
    }

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < Conteneur.Count(); i++)
        {
            Conteneur[i].item = itemDatabaseObject.GetItem[Conteneur[i].ID];
        }
    }

    public void OnBeforeSerialize()
    {
    }
}
[System.Serializable]
public class InventorySlot
{
    public int ID;
    public ItemObject item;
    public int quantite;
    public InventorySlot(int _id,ItemObject objet, int nombre)
    {
        ID = _id;
        item = objet;
        quantite = nombre;
    }

    public void AddAmount(int value) 
    {
        quantite += value;
    }

    public void SetAmount(int value) 
    {
        quantite = value;
    }
}