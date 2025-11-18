using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType {Exp, Level, Kill, Time, Health }
    public InfoType type;

    Text myText;
    Slider mySlider;

     void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

     void LateUpdate()
    {
        switch (type) { 
        case InfoType.Exp:
                float curExp = GameManager.Instance.exp;
                float maxExp = GameManager.Instance.nextExp[GameManager.Instance.level];
                mySlider.value = curExp / maxExp;
                break;
        case InfoType.Level:
                myText.text = string.Format("Lv.{0:F0}", GameManager.Instance.level);   //{0}은 0번쨰 인자값이 들어간다는 뜻, :F0은 소수점이 없다는 뜻
            break;
        case InfoType.Kill:
                myText.text = string.Format("{0:F0}", GameManager.Instance.kill);   //{0}은 0번쨰 인자값이 들어간다는 뜻, :F0은 소수점이 없다는 뜻
                break;
        case InfoType.Time:
                float remainTime = GameManager.Instance.maxGameTime - GameManager.Instance.GameTime;
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec); //D = 자리수
                break;
        case InfoType.Health:
                float curHealth = GameManager.Instance.health;
                float maxHealth = GameManager.Instance.maxHealth;
                mySlider.value = curHealth / maxHealth;
                break;
        }

    }
}
