using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreDisplay : MonoBehaviour
{

    public Highscore highscore;
        
        
    [SerializeField]
    private Text text;

    private String timertext;
    private void Start()
    {
        if(text == null)
        {
            text = GetComponent<Text>();
        }
        ///TODO
        //text.text = highscore.time.ToString();
        timertext = $"{highscore.time:0.00}";
        text.text = timertext;
        
        
        
    }
}
