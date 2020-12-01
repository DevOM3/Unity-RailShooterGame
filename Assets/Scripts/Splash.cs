using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class MusicPlayer : MonoBehaviour
{
    private void Awake()
    {
        int musicPlayers = FindObjectsOfType<MusicPlayer>().Length;

        if (musicPlayers > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

    }
    
}
