using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiCtrl : MonoBehaviour
{
    [SerializeField]
    float vitesse = 4f;

    [SerializeField]
    private Transform[] chemin;
    private int index = 0;

    // Update is called once per frame
    void Update()
    {

        transform.position = Vector3.MoveTowards(transform.position, chemin[index].position, vitesse * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {

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
    }
}
