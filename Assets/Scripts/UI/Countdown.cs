using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    [SerializeField] Text countdownText;
    [SerializeField] AudioClip countdownClip, startClip;
    [SerializeField] Color green, red;
    [SerializeField] int maxCountdown = 3;

    void Start()
    {
        Time.timeScale = 0;
        StartGameButton.OnStartGame += HandleStartGame;
    }

    void OnDestroy()
    {
        StartGameButton.OnStartGame -= HandleStartGame;
    }

    void HandleStartGame(int numOfLaps)
    {
        StartCoroutine(StartLevel());
    }

    IEnumerator StartLevel()
    {
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
}
