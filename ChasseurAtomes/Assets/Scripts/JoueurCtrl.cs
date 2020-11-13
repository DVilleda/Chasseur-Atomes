﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoueurCtrl : MonoBehaviour
{
    public CharacterController manette;

    [SerializeField]
    float vitesse = 12.0f, gravite = -9.81f;
    
    private Vector3 vitesseJoueur;
    public Transform verifierSol;
    public float distanceSol = 0.4f;
    public LayerMask masqueSol;
    private bool toucheSol;
    Animator animator;

    public Inventaire inventaire;
    private Rigidbody joueur;
    // Start is called before the first frame update
    void Start()
    {
        joueur = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        animator.SetBool("ToucheSol",true);
    }

    // Update is called once per frame
    void Update()
    {
        toucheSol = Physics.CheckSphere(verifierSol.position, distanceSol, masqueSol);
        animator.SetBool("ToucheSol", toucheSol);
        if (toucheSol && vitesseJoueur.y < 0) 
        {
            vitesseJoueur.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        manette.Move(move * Time.deltaTime * vitesse);

        //Verifier la direction mouvement pour animation
        if (manette.velocity.z < 0) {
            animator.SetFloat("VitXZ", 0);
            animator.SetFloat("ReculerZ", manette.velocity.z);
        } else {
            animator.SetFloat("VitXZ", Mathf.Abs(manette.velocity.x + manette.velocity.z));
            animator.SetFloat("ReculerZ", 0);
        }

        //TODO Ajouter le jump?

        vitesseJoueur.y += gravite * Time.deltaTime;
        manette.Move(vitesseJoueur * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<Item>();
        if (item) {
            inventaire.AjouterItem(item.item, 1);
            Destroy(other.gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        inventaire.Conteneur.Clear();
    }
}