using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private float timer;
    private TimeSpan time;

    bool active = false;

    private void Update()
    {
        if (active)
        {
            timer += Time.deltaTime;
            time = TimeSpan.FromSeconds(timer);
        }
    }

    public string GetTime()
    {
        int min = time.Minutes >= 99 ? 99 : time.Minutes;
        return string.Format("{0:D2}:{1:D2}:{2:D2}", min, time.Seconds, time.Milliseconds / 10);
    }

    public float GetRawScore()
    {
        return timer;
    }

    public void StartTimer()
    {
        timer = 0;
        time = TimeSpan.Zero;
        active = true;
    }

    public void StopTimer()
    {
        active = false;
    }
}
