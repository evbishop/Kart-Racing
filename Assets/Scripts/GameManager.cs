using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] Text lapText, countdownText, gameResultText;
    [SerializeField] Slider progressSlider;
    [SerializeField] GameObject panelGameOver;
    [SerializeField] AudioClip countdownClip, startClip;
    [SerializeField] Color green, red;
    [SerializeField] int maxCountdown = 3;

    public string LapText
    {
        get { return lapText.text; }
        set { lapText.text = value; }
    }

    public float ProgressSliderValue
    {
        get { return progressSlider.value; }
        set { progressSlider.value = value; }
    }

    public float ProgressSliderMaxValue
    {
        get { return progressSlider.maxValue; }
        set { progressSlider.maxValue = value; }
    }

    void Start()
    {
        Checkpoint.OnPlayerCrossedCheckpoint += HandlePlayerCrossedCheckpoint;
        LapHandle.OnPlayerFinishedLap += HandlePlayerFinishedLap;
        LapHandle.OnGameOver += HandleGameOver;
        StartCoroutine(StartLevel());
    }

    void OnDestroy()
    {
        Checkpoint.OnPlayerCrossedCheckpoint -= HandlePlayerCrossedCheckpoint;
        LapHandle.OnPlayerFinishedLap -= HandlePlayerFinishedLap;
        LapHandle.OnGameOver -= HandleGameOver;
    }

    void HandleGameOver(string winner)
    {
        Time.timeScale = 0;
        AudioSource.PlayClipAtPoint(startClip, Camera.main.transform.position);
        panelGameOver.SetActive(true);
        gameResultText.text = $"{winner} won!";
    }

    void HandlePlayerCrossedCheckpoint(int crossedIndex)
    {
        ProgressSliderValue++;
    }

    void HandlePlayerFinishedLap(string textForUI)
    {
        LapText = textForUI;
        ProgressSliderValue = 0;
    }

    IEnumerator StartLevel()
    {
        Time.timeScale = 0;
        countdownText.color = red;
        for (int countdown = maxCountdown; countdown > 0; countdown--)
        {
            countdownText.text = countdown.ToString();
            AudioSource.PlayClipAtPoint(countdownClip, Camera.main.transform.position);
            yield return new WaitForSecondsRealtime(1f);
        }
        countdownText.color = green;
        countdownText.text = "Start!";
        Time.timeScale = 1;
        AudioSource.PlayClipAtPoint(startClip, Camera.main.transform.position);
        yield return new WaitForSecondsRealtime(0.5f);
        countdownText.text = "";
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
