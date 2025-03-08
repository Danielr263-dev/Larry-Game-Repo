using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance; // Singleton instance to store spawn data
    public string lastExitName; // Stores the name of the last exit used

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps SpawnManager across scenes
        }
        else
        {
            Destroy(gameObject); // Prevents duplicates in new scenes
        }
    }
}
