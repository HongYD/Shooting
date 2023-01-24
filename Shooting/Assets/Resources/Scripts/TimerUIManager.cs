using UnityEngine;
using TMPro;

public class TimerUIManager : MonoBehaviour
{
    public TMP_Text text;
    private int minutes;
    private int seconds;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        GameManager.OnReturnTimeValue += OnGetTimeValue;
    }

    private void OnDisable()
    {
        GameManager.OnReturnTimeValue -= OnGetTimeValue;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
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
}
