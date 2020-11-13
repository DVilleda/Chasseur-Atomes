using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
public class DisplayInventaire : MonoBehaviour
{
    public Inventaire inventaire;

    public int X_Start;
    public int Y_Start;
    public int X_ESPACE_ENTRE_ITEM;
    public int NOMBRE_COLONNES;
    public int Y_ESPACE_ENTRE_ITEM;

    Dictionary<InventorySlot, GameObject> itemsAfficher = new Dictionary<InventorySlot, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        CreerDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    public void CreerDisplay()
    {
        for (int i=0;i<inventaire.Conteneur.Count;i++)
        {
            var obj = Instantiate(inventaire.Conteneur[i].item.prefab, Vector3.zero,Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = inventaire.Conteneur[i].quantite.ToString("n0");

            itemsAfficher.Add(inventaire.Conteneur[i], obj);
        }
    }

    public void UpdateDisplay() 
    {
        for (int i = 0; i < inventaire.Conteneur.Count; i++)
        {
            if (itemsAfficher.ContainsKey(inventaire.Conteneur[i]))
            {
                itemsAfficher[inventaire.Conteneur[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventaire.Conteneur[i].quantite.ToString("n0");
            }
            else 
            {
                var obj = Instantiate(inventaire.Conteneur[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = inventaire.Conteneur[i].quantite.ToString("n0");
                itemsAfficher.Add(inventaire.Conteneur[i], obj);
            }
        }
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3(X_Start + (X_ESPACE_ENTRE_ITEM * (i % NOMBRE_COLONNES)), Y_Start +(-Y_ESPACE_ENTRE_ITEM * (i / NOMBRE_COLONNES)), 0f);
    }
}
