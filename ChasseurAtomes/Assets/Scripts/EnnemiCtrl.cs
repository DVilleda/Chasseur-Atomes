﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiCtrl : MonoBehaviour
{
	//Variable ennemi
    [SerializeField]
    float vitesse = 4f;

    [SerializeField]
    private Transform[] chemin;
    private int index = 0;
    
    [SerializeField]
    private Rigidbody ennemi;

    private void Start()
    {
        if (ennemi == null)
            ennemi = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        if (ennemi.CompareTag("EnnemiV1"))
        {
            transform.position = Vector3.MoveTowards(transform.position, chemin[index].position, vitesse * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
		//Si ennemi V1 mouvement programmé
        if (other.tag == "Chemin")
        {
            if (index < chemin.Length - 1)
            {
                index += 1;
            }
            else
            {
                index = 0;
            }
        }
		//Si ennemi V2 ennemi tombe avec la gravité
        else if (ennemi.CompareTag("EnnemiV2") && other.tag == "Joueur") 
        {
            ennemi.isKinematic = false;
        }
    }
}
