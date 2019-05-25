using System;
using UnityEngine;
using UnityEngine.UI;

public class SceneCounter : MonoBehaviour
{
    [SerializeField]
    private Text text;
    public float timer = 0;
    private String timertext;
    private void Start()
    {
        if(text == null)
        {
            text = GetComponent<Text>();
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        
        //timertext = timer.ToString();
        timertext = $"{timer:0.00}";
        text.text = timertext;



    }
}
