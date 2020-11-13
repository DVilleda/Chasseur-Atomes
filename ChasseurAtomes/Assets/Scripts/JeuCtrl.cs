using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JeuCtrl : MonoBehaviour
{
    [SerializeField]
    GameObject inventaireUI;
    [SerializeField]
    int tempsRestant = 90;
    [SerializeField]
    private Text txtTempsRestant;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DecrementerTemps());
    }

    // Update is called once per frame
    void Update()
    {
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

    public void FinJeu() 
    {

    }
}
