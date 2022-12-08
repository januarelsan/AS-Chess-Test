using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameoverUI : Singleton<GameoverUI>
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Text messageText;

    public void Active(string message){
        messageText.text = message;
        panel.SetActive(true);
    }
}
