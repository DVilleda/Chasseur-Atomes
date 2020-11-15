using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public GameObject inventaireUI, infosInitiales, controlJeu,InteragirBureau,MenuCombiner,GameOver;
    public bool finTuto=false;

    public void finirLeTutorial()
    {
        //Afficher canevas
        if (infosInitiales.activeSelf)
        {
            infosInitiales.SetActive(false);
            controlJeu.SetActive(true);
        }
        else if (controlJeu.activeSelf)
        {
            controlJeu.SetActive(false);
            finTuto = true;
        }
    }
    public void AfficherInventaire() 
    {
        if (inventaireUI.activeSelf)
        {
            inventaireUI.SetActive(false);
        }
        else 
        {
            inventaireUI.SetActive(true);
        }
    }

    public void AfficherIntro()
    {
        if (infosInitiales.activeSelf)
        {
            infosInitiales.SetActive(false);
        }
        else
        {
            infosInitiales.SetActive(true);
        }
    }

    public void AfficherControles()
    {
        if (controlJeu.activeSelf)
        {
            controlJeu.SetActive(false);
        }
        else
        {
            controlJeu.SetActive(true);
        }
    }

    public void AfficherInteragirBureau()
    {
        if (!InteragirBureau.activeSelf)
        {
            InteragirBureau.SetActive(true);
        }
    }

    public void AfficherMenuCombiner() 
    {
        MenuCombiner.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void AfficherGameOver() 
    {
        GameOver.SetActive(true);
    }
}
