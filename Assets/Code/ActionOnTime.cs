using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionOnTime : MonoBehaviour
{
    float timer = 1;
    Action timerCallback;
    bool hasTimeElapsed;

    public ActionOnTime(float delay, Action action)
    {
        timer = delay;
        timerCallback = action;
    }

    void Update()
    {
        if(timer >= 0)
        {
            timer -= Time.deltaTime;
        }

        if (timer <= 0 && !hasTimeElapsed)
        {
            //timerCallback();
            hasTimeElapsed = true;
        }
    }
}
