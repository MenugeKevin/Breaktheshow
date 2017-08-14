using UnityEngine;
using System.Collections;

public static class MathUtility
{
    public static string FormatTime(float _time)
    {
        int intTime = Mathf.FloorToInt(_time);
        int minutes = intTime / 60;
        int seconds = intTime % 60;
        float fraction = _time * 1000;
        fraction = fraction % 1000;
        string timeText = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, fraction);
        return timeText;
    }
}

