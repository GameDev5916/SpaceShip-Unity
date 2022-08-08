using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("Health Bar")]
    public Image healthBarFill;
    public Gradient healthBarColor;     // Color of the health bar depending on the remaining health.
    private float healthBarFillRate;

    [Header("Money")]
    public Text moneyText;
    public Text scoreText;

    [Header("Wave")]
    public Text waveText;
    public Text countdownText;
    public Text scoredownText;

    [Header("Buttons")]
    public GameObject endWaveButtons;

    // Instance
    public static GameUI inst;

    void Awake ()
    {
        // Set the instance to this script.
        inst = this;
    }

    #region Subscribing to Events

    void OnEnable ()
    {
        GameManager.inst.onBeginWave.AddListener(OnBeginWave);
        GameManager.inst.onEndWave.AddListener(OnEndWave);
    }

    void OnDisable ()
    {
        GameManager.inst.onBeginWave.RemoveListener(OnBeginWave);
        GameManager.inst.onEndWave.RemoveListener(OnEndWave);
    }

    #endregion

    void Start ()
    {
        // Begin the game with the buttons active.
        endWaveButtons.gameObject.SetActive(true);
    }

    // Called when the game is started.
    // Initializes the health bar UI.
    public void InitializeHealthBar (int maxHp)
    {
        healthBarFill.fillAmount = 1.0f;
        healthBarFillRate = 1.0f / (float)maxHp;
    }

    // Called when the player takes damage / heals.
    // Updates the health bar UI image.
    public void UpdateHealthBar (int curHp)
    {
        healthBarFill.fillAmount = healthBarFillRate * (float)curHp;
        healthBarFill.color = healthBarColor.Evaluate(healthBarFill.fillAmount);
    }

    // Called when the player gets or spends money.
    // Updates the money text.
    public void UpdateMoneyText (int money)
    {
        moneyText.text = money.ToString();
    }

    public void UpdateScoreText (int score)
    {
        scoreText.text = score.ToString();
    }
    // Called when a wave has begun.
    void OnBeginWave ()
    {
        endWaveButtons.gameObject.SetActive(false);
        StartCoroutine(RunCountdownText());
    }

    // Called when a wave has ended.
    void OnEndWave ()
    {
        endWaveButtons.gameObject.SetActive(true);
        endWaveButtons.GetComponentInChildren<UIButton>().gameObject.SetActive(false);
        StartCoroutine(RunScoredownText());
    }

    IEnumerator RunScoredownText ()
    {
        scoredownText.gameObject.SetActive(true);
        for (int x = GameManager.inst.scoreTime; x > 0; --x)
        {
            scoredownText.text = x.ToString();
            yield return new WaitForSeconds(1.0f);
        }

        scoredownText.gameObject.SetActive(false);
    }
    IEnumerator RunCountdownText ()
    {
        scoredownText.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(true);

        for(int x = GameManager.inst.waveBeginCountdownDuration; x > 0; --x)
        {
            countdownText.text = x.ToString();
            yield return new WaitForSeconds(1.0f);
        }

        countdownText.gameObject.SetActive(false);
        waveText.text = "Wave " + GameManager.inst.curWave;
    }
}