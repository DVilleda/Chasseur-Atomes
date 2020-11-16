using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventaireUI : MonoBehaviour
{
    public Transform ConteneurUI;
    public Inventaire inventaire;
    SlotInventaire[] slots;

    void Start()
    {
		slots = ConteneurUI.GetComponentsInChildren<SlotInventaire>();
		UpdateUI();
    }
    void UpdateUI()
	{
		for (int i = 0; i < slots.Length; i++)
		{
			if (i < inventaire.Conteneur.Count) 
			{
				slots[i].CreerSlot(inventaire.Conteneur[i].item);
			}
			else
			{
				slots[i].ViderSlot();
			}
		}
	}
}
