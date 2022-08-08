using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("Public Values")]
    public int curPlayerMoney;
    public int curPlayerScore;
    public int scoreMode;
    public int scoreTime;

    [Header("Wave")]
    public int curWave;
    public int waveBeginCountdownDuration;

    [Header("Events")]
    public UnityEvent onBeginWave;
    public UnityEvent onEndWave;

    private bool gameOver;      // Is the game over?
    private int dead = 0 ;

    // Instance
    public static GameManager inst;

    void Awake ()
    {
        // Set the instance to this script.
        inst = this;

    }

    void Start ()
    {
        // Set saved volume.
        if (!PlayerPrefs.HasKey("Volume"))
            PlayerPrefs.SetFloat("Volume", 1.0f);

        AudioListener.volume = PlayerPrefs.GetFloat("Volume");
    }

    // Gives the player money (killed enemy, etc).
    public void GiveMoney (int amount)
    {
        dead++;
        curPlayerMoney += amount;

        // Update the money UI.
        GameUI.inst.UpdateMoneyText(curPlayerMoney);
    }

    public void GiveScore (int amount)
    {
        if (scoreMode == 1)
        {
            curPlayerScore += amount;
            // Update the score UI.
            GameUI.inst.UpdateScoreText(curPlayerScore);
        }
    }
    // Takes away money from the player (purchases, etc).
    public void TakeMoney (int amount)
    {
        curPlayerMoney -= amount;

        // The player's money should never go negative.
        if(curPlayerMoney < 0)
            Debug.LogError("Player money has gone negative! Problem in code somewhere.");

        // Update the money UI.
        GameUI.inst.UpdateMoneyText(curPlayerMoney);
    }

    // Called when the player begins a new wave.
    public void BeginNewWave ()
    {
        dead = 0;
        scoreMode = 0;
        curWave++;

        // Have we finished the level?
        if(curWave == LevelManager.inst.curLevel.waves.Length)
        {
            WinGame();
            return;
        }

        if(onBeginWave != null)
            onBeginWave.Invoke();

        // Tell the enemy manager to start spawning enemies.
        EnemyManager.inst.SpawnEnemyWave(LevelManager.inst.curLevel.waves[curWave - 1], (float)waveBeginCountdownDuration);
        EnemyManager.inst.SpawnAsteroidWave(LevelManager.inst.curLevel.waves[curWave - 1], (float)waveBeginCountdownDuration);
    }   

    // Called when an enemy dies.
    // Checks if all enemies are dead and if so, end the wave.
    public void CheckWaveState ()
    {
        if (EnemyManager.inst.remainingEnemies == 0)
        {
            scoreTime = 30 + dead * 5;
            scoreMode = 1;
            EndWave();

            StartCoroutine("changeMode");
        }
    }

    // Called when all enemies are dead.                
    void EndWave ()
    {
        if(onEndWave != null)
            onEndWave.Invoke();
    }

    // Called when the player finishes all of the waves - end the game.
    void WinGame ()
    {
        // If the game is already over, return.
        if(gameOver)
            return;

        gameOver = true;

        EndGameScreenUI.inst.SetEndGameScreen(true, curWave);
    }

    // Called when the player dies - end the game.
    public void PlayerDied ()
    {
        // If the game is already over, return.
        if(gameOver)
            return;

        gameOver = true;

        EndGameScreenUI.inst.SetEndGameScreen(false, curWave);
    }

    IEnumerator changeMode()
    {
        yield return new WaitForSeconds(scoreTime);
        scoreMode = 0;
        BeginNewWave();
    }
}