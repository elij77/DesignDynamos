using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu]

public class gunStats : ScriptableObject
{
    public GameObject gModel;
    [Range(1, 15)] public int shootDamage;
    [Range(0.1f, 3)] public float shootRate;
    [Range(25, 500)] public int shootDist;
    public int ammoCurr;
    public int ammoMax;

    public ParticleSystem hitEffect;
    public AudioClip shootSound;
    [Range(0, 1)] public float shootVolume;
    public Image icon;
}
