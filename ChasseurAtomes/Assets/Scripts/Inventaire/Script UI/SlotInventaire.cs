using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotInventaire : MonoBehaviour
{
    public Inventaire inventaire;
    public Inventaire inventaireCombinaison;
    public Image image;
    public InputField quantite;
    public Button ajouter;
    private ItemObject itemActuel;

    public void CreerSlot(ItemObject item) 
    {
        itemActuel = item;
        image.sprite = item.icon;
        image.enabled = true;
        quantite.interactable = true;
        ajouter.interactable = true;
    }

    public void ViderInventaire() 
    {
        inventaireCombinaison.Conteneur.Clear();
    }

    public void AjouterAtomeCombinaison() 
    {
        if (Int32.TryParse(quantite.text,out Int32 resultat)) 
        {
            inventaireCombinaison.AjouterItem(itemActuel, Int32.Parse(quantite.text));
        }
    }

    public void ViderSlot() 
    {
        itemActuel = null;
        image.sprite = null;
        image.enabled = false;
        quantite.interactable = false;
        ajouter.interactable = false;
    }
}
