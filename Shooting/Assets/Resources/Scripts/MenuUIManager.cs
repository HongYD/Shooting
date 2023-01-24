using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{
    public Button PlayButton;
    public Button ExitButton;
    public TMP_Text HighestScoreText;

    private void Start()
    {
        PlayButton.onClick.AddListener(() => OnPlayButtonClick());
        ExitButton.onClick.AddListener(() => OnExitButtonClick());
        HighestScoreText.text = PlayerPrefs.GetInt("highestScore", 0).ToString();
        CursorManager.instance.SetCursorType(CursorType.CursorNormal);
    }

    public void OnPlayButtonClick()
    {
        SceneLoader.Load(SceneType.GameScene);
    }

    public void OnExitButtonClick()
    {
        Application.Quit();
    }
}
