using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JeuCtrl : MonoBehaviour
{
    public Inventaire inventaireCombinaison;
    public Dropdown OptionChoisi;
    [SerializeField]
    GameObject inventaireUI,infosInitiales,controlJeu;
    [SerializeField]
    int tempsRestant = 91;
    bool finTuto = false;
    [SerializeField]
    private Text txtTempsRestant, txtVies;

    [SerializeField]
    Canvas HUDJeu;
    HUD HUDCtrl;

    JoueurCtrl joueurCtrl;
    private bool btnEspaceUse = false;
    private bool btnInteragir = false;
    // Start is called before the first frame update
    void Start()
    {
        HUDCtrl = HUDJeu.GetComponent<HUD>();
        joueurCtrl = GameObject.Find("Personnage").GetComponent<JoueurCtrl>();
        HUDCtrl.AfficherIntro();
        StartCoroutine(Tutorial());
    }

    // Update is called once per frame
    void Update()
    {
        if (!finTuto && Input.GetAxis("Confirmer") != 0)
        {
            if (!btnEspaceUse)
            {
                HUDCtrl.finirLeTutorial();
                btnEspaceUse = true;
            }
        }
        else if (Input.GetAxis("Confirmer")==0) 
        {
            btnEspaceUse = false;
        }

        if (joueurCtrl.pretCombiner)
        {
            HUDCtrl.AfficherInteragirBureau();
            if (Input.GetAxis("Interaction")!=0)
            {
                Time.timeScale = 0f;
                HUDCtrl.AfficherMenuCombiner();
            }
        }
        else
        {
            HUDCtrl.InteragirBureau.SetActive(false);
        }
        //Mettre a jour le HP restant
        txtVies.text = "Vies : "+joueurCtrl.getErreursRestantes().ToString();
        if (joueurCtrl.getErreursRestantes() == 0)
        {
            FinJeu();
        }

        if (Input.GetKeyDown("p"))
        {
            if (!inventaireUI.activeSelf)
            {
                inventaireUI.SetActive(true);
                Time.timeScale = 0f;
            }
            else {
                inventaireUI.SetActive(false);
                Time.timeScale = 1f;
            }
        }
    }

    private void FixedUpdate()
    {
        
    }

    public IEnumerator Tutorial() 
    {
        yield return new WaitUntil(() => HUDCtrl.finTuto);
        StartCoroutine(DecrementerTemps());
    }

    public IEnumerator DecrementerTemps()
    {
        while (tempsRestant > 0)
        {
            yield return new WaitForSeconds(1f);
            tempsRestant--;
            txtTempsRestant.text = "Temps restant : " + tempsRestant;
        }
        FinJeu();
    }

    public void StartJeu()
    {
        Time.timeScale = 0f;
        //Load Canevas Tutorial
        if (Input.GetKeyDown("space"))
        {
            Debug.Log(Time.timeScale);
            Time.timeScale = 1f;
        }
    }

    public void Combiner()
    {
        bool resultat;
        
        resultat = inventaireCombinaison.creerUnLien(inventaireCombinaison.Conteneur.Count, OptionChoisi.options[OptionChoisi.value].text);
        Debug.Log(OptionChoisi.options[OptionChoisi.value].text);
        Debug.Log(resultat);
    }
    public void AnnulerCombiner() 
    {
        inventaireCombinaison.Conteneur.Clear();
        joueurCtrl.pretCombiner = false;
        HUDCtrl.MenuCombiner.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }
    public void FinJeu() 
    {
        HUDCtrl.AfficherGameOver();
        if (Input.GetKeyDown("space"))
        { 
            //Aller main menu scene
        }
    }

    private void OnApplicationQuit()
    {
        inventaireCombinaison.Conteneur.Clear();
    }
}
