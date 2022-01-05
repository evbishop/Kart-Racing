using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverHandler : MonoBehaviour
{
    [SerializeField] GameObject panelGameOver;
    [SerializeField] Text gameResultText;
    [SerializeField] AudioClip finishClip;

    void Start()
    {
        LapHandler.OnGameOver += HandleGameOver;
    }

    void OnDestroy()
    {
        LapHandler.OnGameOver -= HandleGameOver;
    }

    void HandleGameOver(string winner)
    {
        Time.timeScale = 0;
        AudioSource.PlayClipAtPoint(finishClip, Camera.main.transform.position);
        panelGameOver.SetActive(true);
        gameResultText.text = $"{winner} won!";
    }
}
