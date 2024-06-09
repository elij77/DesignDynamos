using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpoint : MonoBehaviour
{
    [SerializeField] Renderer checkpointModel;
    [SerializeField] fadeUI fadeScript;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && gameManager.instance.playerSpawnPos.transform.position != transform.position)
        {
            gameManager.instance.playerSpawnPos.transform.position = transform.position;
            StartCoroutine(displayCheckpointPopup());
        }
    }

    IEnumerator displayCheckpointPopup()
    {
        checkpointModel.material.color = Color.clear;

        fadeScript.ShowUI();
        yield return new WaitForSeconds(2);
        fadeScript.HideUI();

        checkpointModel.material.color = Color.white;
    }
}
