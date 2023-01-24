using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ForceState
{
    Increase,
    Decrease,
    Stop,
}

public class UIManager : MonoBehaviour
{
    public GameObject ForceUI;
    public GameObject EndUIPanel;
    public Image Force;
    public Button HomeButton;
    public Button ReplayButton;
    public TMP_Text highestScoreText;
    public TMP_Text currentScoreText;
    public bool isShowForce;
    public ForceState state;
    public float speed = 0.1f;
    public float fillRate;
    // Start is called before the first frame update

    private void Awake()
    {

    }

    private void OnEnable()
    {
        PlayerStateManager.OnPlayerHoldBall += OnHold;
        PlayerStateManager.OnPlayerShootBall += OnShoot;
        GameManager.OnGameEndEvent += this.OnGameEndEvent;
    }

    private void OnDisable()
    {
        PlayerStateManager.OnPlayerHoldBall -= OnHold;
        PlayerStateManager.OnPlayerShootBall -= OnShoot;
        GameManager.OnGameEndEvent -= this.OnGameEndEvent;
    }

    void Start()
    {
        EndUIPanel.gameObject.SetActive(false);
        Force.fillAmount = 0;
        state= ForceState.Stop;
    }

    // Update is called once per frame
    void Update()
    {
        changeState();
    }

    private void changeState()
    {
        switch (state)
        {
            case ForceState.Increase:
                StartCoroutine(OnIncrease());
                break;
            case ForceState.Decrease:
                StartCoroutine(OnDecrease());
                break;
            case ForceState.Stop:
                OnStop();
                break;
        }
    }

    private IEnumerator OnIncrease()
    {
        Force.fillAmount += speed * Time.deltaTime;
        fillRate = Force.fillAmount;
        yield return new WaitForSeconds(0.1f);
        if(Force.fillAmount >= 1.0f)
        {
            Force.fillAmount = 1.0f;
            fillRate = Force.fillAmount;
            state = ForceState.Decrease;
        }
    }

    private IEnumerator OnDecrease()
    {
        Force.fillAmount -= speed * Time.deltaTime;
        fillRate = Force.fillAmount;
        yield return new WaitForSeconds(0.1f);
        if (Force.fillAmount <= 0)
        {
            Force.fillAmount = 0;
            fillRate = Force.fillAmount;
            state = ForceState.Increase;
        }
    }


    public void OnHold()
    {
        state = ForceState.Increase;
    }

    public float OnShoot()
    {
        state = ForceState.Stop;
        return fillRate;
    }

    private void OnStop()
    {

    }

    private void OnHomeButtonClick()
    {
        SceneLoader.Load(SceneType.MenuScene);
    }

    private void OnReplayButtonClick()
    {
        SceneLoader.Load(SceneType.GameScene);
    }

    public void OnGameEndEvent(int score,int highestScore)
    {
        EndUIPanel.gameObject.SetActive(true);
        HomeButton.onClick.AddListener(() => OnHomeButtonClick());
        ReplayButton.onClick.AddListener(() => OnReplayButtonClick());
        highestScoreText.text = highestScore.ToString();
        currentScoreText.text = score.ToString();       
    }

}
