using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JeuCtrl : MonoBehaviour
{
    JoueurCtrl joueurCtrl;
    // Start is called before the first frame update
    void Start()
    {
        joueurCtrl = GameObject.Find("Joueur").GetComponent<JoueurCtrl>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }
}
