using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fadeUI : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroupUI;

    [SerializeField] float fadeDur = 1f;

    [SerializeField] bool fadeIn = false;
    [SerializeField] bool fadeOut = false;

    public void ShowUI()
    {
        fadeIn = true;
    }

    public void HideUI()
    {
        fadeOut = true;
    }

    private void Update()
    {
        if (fadeIn)
        {
            if (canvasGroupUI.alpha < 1)
            {
                canvasGroupUI.alpha += Time.deltaTime / fadeDur;
                if(canvasGroupUI.alpha >= 1)
                {
                    fadeIn = false;
                }
            }
        }

        if (fadeOut)
        {
            if (canvasGroupUI.alpha >= 0)
            {
                canvasGroupUI.alpha -= Time.deltaTime / fadeDur;
                if (canvasGroupUI.alpha == 0)
                {
                    fadeOut = false;
                }
            }
        }
    }
}
