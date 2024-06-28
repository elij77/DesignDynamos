using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIHealth : MonoBehaviour
{
//    [SerializeField] private Slider slider;

    [SerializeField] private Image healthBar;
    [SerializeField] private Image damageBar;


    //[SerializeField] private Transform position;

    //private Camera cam;

    public void updateHealthBar(float HP, float maxHP)
    {
        float healthPercentage = HP / maxHP;
        healthBar.fillAmount = healthPercentage;
        StartCoroutine(UpdateDamageIndicator(healthPercentage));
    }

    private void Start()
    {
        //cam = Camera.main;
    }
    void Update()
    {
        //transform.rotation = cam.transform.rotation;
    }

    public IEnumerator UpdateDamageIndicator(float targetValue)
    {
        yield return new WaitForSeconds(0.5f);

        while (damageBar.fillAmount > targetValue)
        {
            damageBar.fillAmount -= Time.deltaTime / 2f;
            yield return null;
        }
    }

}
