using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{

    [SerializeField] int sens;

    [SerializeField] int lockVertMin, lockVertMax;

    [SerializeField] bool invertY;

    float rotX;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;

        Cursor.lockState = CursorLockMode.Locked; 
    }

    // Update is called once per frame
    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sens;

        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sens;

        if (invertY)
        {
            rotX += mouseY;
        }
        else
        {
            rotX -= mouseY;
        }

        rotX = Mathf.Clamp(rotX, lockVertMin, lockVertMax);

        transform.localRotation = Quaternion.Euler(rotX, 0, 0);

        transform.parent.Rotate(Vector3.up * mouseX);
        

    }

    
}
