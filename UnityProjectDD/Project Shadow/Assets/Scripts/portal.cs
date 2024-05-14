using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class portal : MonoBehaviour
{

    [SerializeField] string scene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            SceneManager.LoadScene(scene);
            // scenes need to be added to the build.
            // go the scene you want to add.  
            //file->BuildSetting hit the add scene button.
        }
    }


}
