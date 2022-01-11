using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGameButton : MonoBehaviour
{
    [SerializeField] InputField lapsInput;

    public static event Action<int> OnStartGame;

    public void StartGame()
    {
        int lapsAmount;
        if (lapsInput.text == ""
            || lapsInput.text == "0"
            || !int.TryParse(lapsInput.text, out lapsAmount))
            lapsAmount = 1;
        OnStartGame?.Invoke(lapsAmount);
        transform.parent.gameObject.SetActive(false);
    }
}
