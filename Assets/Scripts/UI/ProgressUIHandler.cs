using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressUIHandler : MonoBehaviour
{
    [SerializeField] Text lapText;
    [SerializeField] Slider progressSlider;
    
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

    void HandleLapsTextUpdated(string text, int sliderMaxValue)
    {
        LapText = text;
        ProgressSliderMaxValue = sliderMaxValue;
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
}
