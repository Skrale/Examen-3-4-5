using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceManager : MonoBehaviour {

    public Text raceTime1;
    public Text raceTime2;
    public Text lapCounter1;
    public Text lapCounter2;

    int lapCount;
    float secondsCount;
    int minuteCount;

    bool canFinish = false;
    

	void Start ()
    {
        lapCounter1.text = "Lap: " + lapCount + "/3";
	}
	

	void Update ()
    {
        secondsCount += Time.deltaTime;
        raceTime1.text = minuteCount + "m - " +(int)secondsCount + "s ";
        if(secondsCount >= 60)
        {
            minuteCount++;
            secondsCount = 0;
        }
	}
}
