using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoueurCtrl : MonoBehaviour
{
    public CharacterController manette;
    [SerializeField]
    private Transform positionInitiale;
    [SerializeField]
    private GameObject joueur;
    [SerializeField]
    public int erreursRestantes = 1;

    [SerializeField]
    float vitesse = 12.0f, gravite = -9.81f;

    private Vector3 vitesseJoueur;
    public Transform verifierSol;
    public float distanceSol = 0.4f;
    public LayerMask masqueSol;
    private bool toucheSol;
    Animator animator;

    public bool pretCombiner = false;

    public Inventaire inventaire;
    public SonSFXCtrl ctrlSon;
    public HUD hudctrl;
    // Start is called before the first frame update
    void Start()
    {
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
        animator.SetFloat("VitXZ", Mathf.Abs(manette.velocity.x + manette.velocity.z));

        //TODO Ajouter le jump?
        vitesseJoueur.y += gravite * Time.deltaTime;
        manette.Move(vitesseJoueur * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        
    }

    //Retourner le nombre de vies qui reste
    public int getErreursRestantes()
    {
        return erreursRestantes;
    }

    private void PunitionSurveillant() 
    {
        ctrlSon.PerdreUneVie();
        transform.position = positionInitiale.transform.position;
        erreursRestantes--;
    }

    public void OnCollisionEnter(Collision collision)
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnnemiV1")
        {
            PunitionSurveillant();
        } else if (other.tag == "Bureau")
        {
            pretCombiner = true;
        }

        var item = other.GetComponent<Item>();
        if (item) {
            ctrlSon.RammasserAtome();
            hudctrl.afficherItemPickup();
            inventaire.AjouterItem(item.item, 1);
            Destroy(other.gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Bureau")
        {
            pretCombiner = false;
        }
    }

    private void OnApplicationQuit()
    {
        inventaire.Conteneur.Clear();
    }
}
