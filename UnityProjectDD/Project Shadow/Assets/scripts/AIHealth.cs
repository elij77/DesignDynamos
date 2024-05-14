using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIHealth : MonoBehaviour
{
    [SerializeField] private Slider slider;
    
    [SerializeField] private Transform position;

    private Camera cam;

    public void updateHealthBar(float HP, float maxHP)
    {
        slider.value = HP / maxHP;
    }

    private void Start()
    {
        cam = Camera.main;
    }
    void Update()
    {
        transform.rotation = cam.transform.rotation;


    }
}
