using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public enum SceneType
{
    None,
    MenuScene,
    GameScene,
}

public class GameManager : MonoBehaviour
{
    public float time;
    public int score;
    public bool isGameEnd;
    private GameObject ballPivot;

    public delegate void ReturnTimeValueEvent(float timeValue);
    public static event ReturnTimeValueEvent OnReturnTimeValue;

    public delegate void ReturnScoreEvent(int socreValue);
    public static event ReturnScoreEvent OnReturnScore;

    public static event Action<int,int> OnGameEndEvent;

    private void OnEnable()
    {
        BallStateManager.OnReturnScore += OnSocre;
    }

    private void OnDisable()
    {
        BallStateManager.OnReturnScore -= OnSocre;
    }

    private void Start()
    {
        time = 180.0f;
        score = 0;
        ballPivot = GameObject.Find("BallPivot");
        isGameEnd = false;
    }


    private void Update()
    {
        if(time > 0)
        {
            time-= Time.deltaTime;
        }
        else
        {
            time = 0;
        }
        if (OnReturnTimeValue != null)
        {
            OnReturnTimeValue(time);
        }
        if (OnReturnScore != null)
        {
            OnReturnScore(score);
        }

        if (!isGameEnd)
        {
            if ((time <= 0 || ballPivot.transform.childCount == 0 || Keyboard.current.cKey.wasPressedThisFrame))
            {
                isGameEnd = true;
                onGameEnd();
            }
        }
        else
        {
            return;
        }
    }

    private void OnSocre(int scoreValue)
    {
        score += scoreValue;
    }

    private void onGameEnd()
    {
        int highestScore = PlayerPrefs.GetInt("highestScore", 0);
        if(highestScore < score)
        {
            PlayerPrefs.SetInt("highestScore", score);
        }
        if (OnGameEndEvent != null)
        {
            OnGameEndEvent(score, highestScore);
        }
    }
}
