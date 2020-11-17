using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonSFXCtrl : MonoBehaviour
{
    [SerializeField]
    AudioSource SonRammasser,PerdreVie,PerdreJeu,Victoire;

    public void RammasserAtome() 
    {
        SonRammasser.Play();
    }

    public void PerdreUneVie() 
    {
        PerdreVie.Play();
    }

    public void ChargerSonPerdre()
    {
        PerdreJeu.Play();
    }

    public void GangerPartie() 
    {
        Victoire.Play();
    }
}
