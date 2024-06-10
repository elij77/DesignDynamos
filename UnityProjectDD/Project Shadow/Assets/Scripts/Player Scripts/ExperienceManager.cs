using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    public static ExperienceManager instance;

    public delegate void ExperienceChangeHandler(int amount);
    public event ExperienceChangeHandler onExperienceChange;
       

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public void AddExperience(int amount)
    {
        onExperienceChange?.Invoke(amount);
    }
}
