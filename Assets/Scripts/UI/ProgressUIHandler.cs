using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressUIHandler : MonoBehaviour
{
    [SerializeField] Text lapText;
    [SerializeField] Slider progressSlider;
    int numOfLaps = 0;
    
    public string LapText
    {
        get { return lapText.text; }
        set { lapText.text = $"Lap: {value}/{numOfLaps}"; }
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
        CarProgressHandler.OnPlayerCrossedCheckpoint += HandlePlayerCrossedCheckpoint;
        CarProgressHandler.OnPlayerFinishedLap += HandlePlayerFinishedLap;
        LapHandler.OnLapsTextUpdated += HandleLapsTextUpdated;
    }

    void OnDestroy()
    {
        CarProgressHandler.OnPlayerCrossedCheckpoint -= HandlePlayerCrossedCheckpoint;
        CarProgressHandler.OnPlayerFinishedLap -= HandlePlayerFinishedLap;
        LapHandler.OnLapsTextUpdated -= HandleLapsTextUpdated;
    }

    void HandleLapsTextUpdated(int currentLap, int numOfLaps, int sliderMaxValue)
    {
        this.numOfLaps = numOfLaps;
        LapText = currentLap.ToString();
        ProgressSliderMaxValue = sliderMaxValue;
    }

    void HandlePlayerCrossedCheckpoint(int crossedIndex)
    {
        ProgressSliderValue++;
    }

    void HandlePlayerFinishedLap(int currentLap)
    {
        if (currentLap > numOfLaps) return;
        LapText = currentLap.ToString();
        ProgressSliderValue = 0;
    }
}
