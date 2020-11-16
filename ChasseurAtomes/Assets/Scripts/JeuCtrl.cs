using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JeuCtrl : MonoBehaviour
{
    public Inventaire inventaireCombinaison;
    public Dropdown OptionChoisi;
    [SerializeField]
    int tempsRestant = 91;
    int TempsTotal;
    int pointage;
    bool finTuto = false;

    [SerializeField]
    Canvas HUDJeu;
    HUD HUDCtrl;
    [SerializeField]
    private Text txtTempsRestant, txtVies;
    public SonSFXCtrl ctrlSon;

    JoueurCtrl joueurCtrl;
    private bool btnEspaceUse = false;
    private bool finJeu = false;
    // Start is called before the first frame update
    void Start()
    {
        TempsTotal = tempsRestant - 1;
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
        else if (Input.GetAxis("Confirmer") == 0)
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
            if (!HUDCtrl.inventaireUI.activeSelf)
            {
                HUDCtrl.inventaireUI.SetActive(true);
                Time.timeScale = 0f;
            }
            else {
                HUDCtrl.inventaireUI.SetActive(false);
                Time.timeScale = 1f;
            }
        }

        if (finJeu && Input.GetKeyDown("space"))
        {
            LoadMenu();
        }
    }

    private void FixedUpdate()
    {
        
    }

    void LoadMenu() 
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
    public void AnnulerCombiner() 
    {
        HUDCtrl.InteragirBureau.SetActive(true);
        inventaireCombinaison.Conteneur.Clear();
        joueurCtrl.pretCombiner = false;
        HUDCtrl.MenuCombiner.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }
    public void FinJeu() 
    {
        Cursor.lockState = CursorLockMode.None;
        HUDCtrl.ReponseNoOK.SetActive(false);
        HUDCtrl.AfficherGameOver();
        ctrlSon.ChargerSonPerdre();
        finJeu = true;
    }

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
}
