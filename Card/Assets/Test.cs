using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour {
    private long time = 200000000;
    public Text txtTime;
    float ShowTime;
    bool isTimerShow;
    int timer = 0;
    // Use this for initialization
    void Start () {
        ShowTimer(time);
    }

     void ShowTimer(long time)
    {
        Debug.Log("显示计时器 " + time);
        //转换为秒
        ShowTime = time / 10000000.0f;
        setTimerActive(true);
        isTimerShow = true;
    }

    protected void setTimerActive(bool active)
    {
        txtTime.gameObject.SetActive(active);
    }

    // Update is called once per frame
    void Update () {
        if(isTimerShow)
        {
            ShowTime = ShowTime - Time.deltaTime;
            if(ShowTime<0)
            {
                setTimerActive(false);
                isTimerShow = false;
            }
            UpdateTime(ShowTime);
        }
    }

    private void UpdateTime(float time)
    {
        int tTime = (int)time;
        if (tTime != timer)
        {
            if (tTime < 0)
                return;
            txtTime.text = tTime.ToString();
            timer = tTime; ;
        }
    }

}
