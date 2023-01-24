using UnityEngine;
using TMPro;

public class ScoreboardUIManager : MonoBehaviour
{
    public TMP_Text textTime;
    public TMP_Text textScore;
    private int minutes;
    private int seconds;
    private int score;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        GameManager.OnReturnTimeValue += OnGetTimeValue;
        GameManager.OnReturnScore += OnGetScoreValue;
    }

    private void OnDisable()
    {
        GameManager.OnReturnTimeValue -= OnGetTimeValue;
        GameManager.OnReturnScore -= OnGetScoreValue;
    }

    // Update is called once per frame
    void Update()
    {
        textTime.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        textScore.text = score.ToString();
    }

    public void OnGetTimeValue(float time)
    {
        if (time < 0)
        {
            time = 0;
        }

        minutes = Mathf.FloorToInt(time / 60);
        seconds = Mathf.FloorToInt(time % 60);
    }

    public void OnGetScoreValue(int scoreValue)
    {
        score = scoreValue;
    }
}
