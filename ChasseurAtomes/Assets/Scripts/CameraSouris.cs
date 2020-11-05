using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSouris : MonoBehaviour
{
    [SerializeField]
    float sensiviteSouris = 100f;

    public Transform jouerbody;

    private float Rotationx = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensiviteSouris * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensiviteSouris * Time.deltaTime;

        Rotationx -= mouseY;
        Rotationx = Mathf.Clamp(Rotationx, -90f, 90f);

        transform.localRotation = Quaternion.Euler(Rotationx, 0f, 0f);
        jouerbody.Rotate(Vector3.up * mouseX);
    }
}
