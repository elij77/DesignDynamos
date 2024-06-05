using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ammoTrack : MonoBehaviour
{
    public gunStats gun;
    public TMP_Text ammoText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateAmmo();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAmmo();
    }

    public void UpdateAmmo()
    {
        ammoText.text = gun.ammoCurr.ToString() + '/' + gun.ammoMax.ToString();
    }
}
