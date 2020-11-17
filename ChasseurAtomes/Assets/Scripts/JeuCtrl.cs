using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JeuCtrl : MonoBehaviour
{
	//Variables Interface Combiner atomes
    public Inventaire inventaireCombinaison;
    public Dropdown OptionChoisi;
	
	//Variables du jeu
    [SerializeField]
    int tempsRestant = 91;
    int TempsTotal;
    int pointage;
    bool finTuto = false;
	
	//Variables de l'interface graphique
    [SerializeField]
    Canvas HUDJeu;
    HUD HUDCtrl;
    [SerializeField]
    private Text txtTempsRestant, txtVies;
    public SonSFXCtrl ctrlSon;
	
	//Variables pour le joueur
    JoueurCtrl joueurCtrl;
    private bool btnEspaceUse = false;
    private bool finJeu = false;
    // Start is called before the first frame update
    void Start()
    {
        TempsTotal = tempsRestant - 1;
        HUDCtrl = HUDJeu.GetComponent<HUD>();
        joueurCtrl = GameObject.Find("Personnage").GetComponent<JoueurCtrl>();
		//Commencer l'introduction
        HUDCtrl.AfficherIntro();
        StartCoroutine(Tutorial());
    }

    // Update is called once per frame
    void Update()
    {
		//Avancer a travers le tuto
        if (!finTuto && Input.GetAxis("Confirmer") != 0)
        {
            if (!btnEspaceUse)
            {
                HUDCtrl.finirLeTutorial();
                btnEspaceUse = true;
            }
        }
        else if (Input.GetAxis("Confirmer") == 0)
        {
            btnEspaceUse = false;
        }
		//Commencer la combinaison d'atomes
        if (joueurCtrl.pretCombiner)
        {
            HUDCtrl.AfficherInteragirBureau();
            if (Input.GetAxis("Interaction")!=0)
            {
                Time.timeScale = 0f;
                HUDCtrl.AfficherMenuCombiner();
                Cursor.visible = true;
            }
        }
        else
        {
            HUDCtrl.InteragirBureau.SetActive(false);
        }
		//Voir une lecon selon le collider
        if (joueurCtrl.leconCovalent || joueurCtrl.leconIonique)
        {
            HUDCtrl.interagirPoster.SetActive(true);
            if (Input.GetKeyDown("r"))
            {
                if (joueurCtrl.leconCovalent)
                {
                    if (!HUDCtrl.LeconCovalente.activeSelf)
                    {
                        HUDCtrl.LeconCovalente.SetActive(true);
                        Time.timeScale = 0f;
                    }
                    else 
                    {
                        Time.timeScale = 1f;
                        HUDCtrl.LeconCovalente.SetActive(false);
                        joueurCtrl.leconCovalent = false;
                    }
                } else if (joueurCtrl.leconIonique)
                {
                    if (!HUDCtrl.LeconIonique.activeSelf)
                    {
                        HUDCtrl.LeconIonique.SetActive(true);
                        Time.timeScale = 0f;

                    }
                    else
                    {
                        Time.timeScale = 1f;
                        HUDCtrl.LeconIonique.SetActive(false);
                        joueurCtrl.leconIonique = false;
                    }
                }
            }
        }
        else
        {
            HUDCtrl.interagirPoster.SetActive(false);
        }

        //Mettre a jour le HP restant
        txtVies.text = "Vies : "+joueurCtrl.getErreursRestantes().ToString();
        if (joueurCtrl.getErreursRestantes() == 0)
        {
            FinJeu();
        }
		//Ouvrir menu pause
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            MenuPause();
        }
		//Ouvrir inventaire
        if (Input.GetKeyDown("p"))
        {
            if (!HUDCtrl.inventaireUI.activeSelf)
            {
                HUDCtrl.inventaireUI.SetActive(true);
            }
            else {
                HUDCtrl.inventaireUI.SetActive(false);
            }
        }
		//Finir le jeu et aller au menu principal
        if (finJeu && Input.GetKeyDown("space"))
        {
            LoadMenu();
        }
    }

    private void FixedUpdate()
    {
        
    }
	
	//Charger le menu principal
    public void LoadMenu() 
    {
        StartCoroutine(ChargerMenu());
    }

    public IEnumerator ChargerMenu() 
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Main Menu");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        HUDCtrl.MenuPause.SetActive(false);
    }
	
	//Coroutine du tutoriel et commencer le timer
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
	
	//Invoquer une combinaison et validation de la reponse
    public void Combiner()
    {
        bool resultat;
        
        resultat = inventaireCombinaison.creerUnLien(inventaireCombinaison.Conteneur.Count, OptionChoisi.options[OptionChoisi.value].text);
        Debug.Log(resultat);
        if (resultat)
        {
            HUDCtrl.ReponseOK.SetActive(true);
        } else if (!resultat && joueurCtrl.getErreursRestantes()>0) 
        {
            ctrlSon.PerdreUneVie();
            joueurCtrl.erreursRestantes--;
            HUDCtrl.ReponseNoOK.SetActive(true);
        }
    }
	
	//Calcul du pointage
    public void CalculerPointage() 
    {
        int pointsTemps = Mathf.Abs(tempsRestant - TempsTotal);
        int pointsVie = (joueurCtrl.getErreursRestantes()*10);
        pointage = pointsTemps + pointsVie;
    }
	
    public void ReponseIncorrecte() 
    {
        HUDCtrl.ReponseNoOK.SetActive(false);
        AnnulerCombiner();
    }
	
	//Annuler l'action de combiner
    public void AnnulerCombiner() 
    {
        HUDCtrl.InteragirBureau.SetActive(true);
        inventaireCombinaison.Conteneur.Clear();
        joueurCtrl.pretCombiner = false;
        HUDCtrl.MenuCombiner.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }
	//Finir le jeu avec defaite
    public void FinJeu() 
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        HUDCtrl.ReponseNoOK.SetActive(false);
        HUDCtrl.AfficherGameOver();
        ctrlSon.ChargerSonPerdre();
        finJeu = true;
    }
	
	//Gagner le jeu avec une bonne reponse
    public void GagnerJeu() 
    {
        StopCoroutine(DecrementerTemps());
        int pointsTemps = tempsRestant;
        int pointsVie = (joueurCtrl.getErreursRestantes() * 10);
        pointage = pointsTemps + pointsVie;
        string msg = "Vous avez réussi avec "+ tempsRestant + " secondes restantes et "+ joueurCtrl.getErreursRestantes() + " vies restantes. Votre score final est "+pointage+" .";
        HUDCtrl.AfficherEcranVictoire(msg);
        ctrlSon.GangerPartie();
        inventaireCombinaison.Conteneur.Clear();
        finJeu = true;
    }

    private void OnApplicationQuit()
    {
        inventaireCombinaison.Conteneur.Clear();
    }

    public void MenuPause() 
    {
        if (!HUDCtrl.MenuPause.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            HUDCtrl.MenuPause.SetActive(true);
            Time.timeScale = 0f;
        }
        else 
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            HUDCtrl.MenuPause.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void Quitter() 
    {
        Application.Quit();
    }
}
