using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[CreateAssetMenu(fileName ="Nouvel Inventaire",menuName ="Inventory System/Inventaire")]
public class Inventaire : ScriptableObject, ISerializationCallbackReceiver
{
	//Classe inventaire qui va contenir nos objets
    public string savePath;
	//Lien vers une BD
    public ItemDatabaseObject itemDatabaseObject;
	//Liste des conteneurs d'objets dans l'inventaire
    public List<InventorySlot> Conteneur = new List<InventorySlot>();

	//Fonction qui verifie si l'item existe deja et augmente la quantite ou cree un item
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
	
	//Fonction qui verifie s'occupe de cree un lien en comptant les electrons et les items soumis
    public bool creerUnLien(int TailleInv,string typeLien) 
    {
        if (Conteneur.Count<1) 
        {
            return false;
        }
        int TotalElectrons = 0;
		//Si c'est un lien ionique on utilise modulo 8
        if (typeLien.Equals("Ionique"))
        {
            if (Conteneur.Count<2) 
            {
                return false;
            }
            for (int i=0;i<TailleInv;i++) 
            {
                TotalElectrons += (Conteneur[i].quantite * Conteneur[i].item.electrons);
            }
            if (TotalElectrons % 8 == 0)
            {
                return true;
            }
        }
		//Si c'est un lien covelent on utilise modulo 2
        else if (typeLien.Equals("Covalente"))
        {
            for (int i = 0; i < TailleInv; i++)
            {
                if (Conteneur[i].item.electrons < 4)
                {
                    return false;
                }
                TotalElectrons += (Conteneur[i].quantite * Conteneur[i].item.electrons) % 2;
            }
            if (TotalElectrons%2==0)
            {
                return true;
            }
        }
        return false;
    }
	
	//Fonction pour sauvegarder les donnees 
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
//Classe en charge de creer les conteneur d'item dans l'inventaire
[System.Serializable]
public class InventorySlot
{
    public int ID;
    public ItemObject item;
    public int quantite;
	//Creer un conteneur d'objet avec les params
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