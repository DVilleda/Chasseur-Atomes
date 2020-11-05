using System.Collections;
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

    private Rigidbody joueur;
    // Start is called before the first frame update
    void Start()
    {
        joueur = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        toucheSol = Physics.CheckSphere(verifierSol.position, distanceSol, masqueSol);
        if (toucheSol && vitesseJoueur.y < 0) 
        {
            vitesseJoueur.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        manette.Move(move * Time.deltaTime * vitesse);

        //TODO Ajouter le jump?

        vitesseJoueur.y += gravite * Time.deltaTime;
        manette.Move(vitesseJoueur * Time.deltaTime);
    }
}
