using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveValues : MonoBehaviour
{
    public static SaveValues instance;
    SWMovement sWMovement;
    public float savedVolume;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        sWMovement = FindObjectOfType<SWMovement>();
    }

    private void Update()
    {
        if (sWMovement != null)
        {
            savedVolume = sWMovement.volumePublic;
        }
    }
}
