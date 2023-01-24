using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundAudioClip
{
    public SoundManager.SoundType type;
    public AudioClip audioClip;
}

public class GameAsstes : MonoBehaviour
{
    private static GameAsstes _instance;

    public static GameAsstes Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = Instantiate(Resources.Load<GameAsstes>("Prefab/GameAssets"));
            }
            return _instance;
        }
    }

    public SoundAudioClip[] soundAudioClips;
    public GameObject basketball;
    public GameObject football;
}
