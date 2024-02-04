using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType {Exp, Level, Kill, Time}

    public InfoType type;

    private Text myText;
    private Slider mySlider;

    private void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        if (InGameManager.Instance.isPaused)
            return;
        
        switch (type)
        {
            case InfoType.Exp:
                float currExp = InGameManager.Instance.currExp;
                float maxExp = InGameManager.Instance.maxExp[InGameManager.Instance.playerLevel];
                mySlider.value = currExp / maxExp;
                break;
            case InfoType.Level:
                myText.text = string.Format("Lv.{0:F0}", InGameManager.Instance.playerLevel);
                break;
            case InfoType.Kill:
                myText.text = string.Format("{0:F0}", InGameManager.Instance.killCount);
                break;
            case InfoType.Time:
                float playtime = InGameManager.Instance.gameTimer;
                int min = Mathf.FloorToInt(playtime / 60);
                int sec = Mathf.FloorToInt(playtime % 60);
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;
        }
    }
}
