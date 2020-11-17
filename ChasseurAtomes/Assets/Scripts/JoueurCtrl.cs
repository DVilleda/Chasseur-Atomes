using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoueurCtrl : MonoBehaviour
{
	//Variables joueur
    public CharacterController manette;
    [SerializeField]
    private Transform positionInitiale;
    [SerializeField]
    private GameObject joueur;
    [SerializeField]
    public int erreursRestantes;

    [SerializeField]
    float vitesse = 12.0f, gravite = -9.81f;
	
	//Variables joueur position
    private Vector3 vitesseJoueur;
    public Transform verifierSol;
    public float distanceSol = 0.4f;
    public LayerMask masqueSol;
    private bool toucheSol;
	//Animateur
    Animator animator;
	
	//variable verifier position relative aux objets
    public bool pretCombiner = false;
    public bool leconIonique = false;
    public bool leconCovalent = false;

	//Inventaire et controleurs Interface graphique
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
		//Verifier si touche sol
        toucheSol = Physics.CheckSphere(verifierSol.position, distanceSol, masqueSol);
        animator.SetBool("ToucheSol", toucheSol);
        if (toucheSol && vitesseJoueur.y < 0) 
        {
            vitesseJoueur.y = -2f;
        }
		
		//Gestion du mouvement avec le controller
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        manette.Move(move * Time.deltaTime * vitesse);

        //Verifier la direction mouvement pour animation    
        animator.SetFloat("VitXZ", Mathf.Abs(manette.velocity.x + manette.velocity.z));

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
	
	//Toucher un ennemi
    private void PunitionSurveillant() 
    {
        ctrlSon.PerdreUneVie();
        transform.position = positionInitiale.transform.position;
        erreursRestantes--;
    }

	
    public void OnTriggerEnter(Collider other)
    {
		//Verifier si la collison est avec un ennemi
        if (other.tag == "EnnemiV1" || other.tag == "EnnemiV2")
        {
            Debug.Log(other.gameObject.name);
            PunitionSurveillant();
        }
		//Verifier si on touche le collider du bureau
        else if (other.tag == "Bureau" && inventaire.Conteneur.Count>0)
        {
            pretCombiner = true;
        }
		//Verifier si on est devant une affiche de lecon
        else if (other.tag == "Ionique")
        {
            leconIonique = true;
        }
        else if (other.tag == "Covalente")
        {
            leconCovalent = true;
        }
	
		//Verifier si on a touche un atome et ajout a l'inventaire
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
        else if (other.tag == "Ionique")
        {
            leconIonique = false;
        }
        else if (other.tag == "Covalente")
        {
            leconCovalent = false;
        }
    }

    private void OnApplicationQuit()
    {
        inventaire.Conteneur.Clear();
    }
}
