using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceManager : MonoBehaviour {

    public Text raceTime1;
    public Text raceTime2;
    public Text lapCounter1;
    public Text lapCounter2;
    bool hasFinished1 = false;

    public int lapCount;
    float secondsCount;
    int minuteCount;

    public bool canFinish = false;
    

	void Start ()
    {
        lapCount++;
	}
	

	void Update ()
    {
        if (!hasFinished1)
        {
            secondsCount += Time.deltaTime;
            raceTime1.text = minuteCount + "m - " + (int)secondsCount + "s ";
            if (secondsCount >= 60)
            {
                minuteCount++;
                secondsCount = 0;
            }
        }

        lapCounter1.text = "Lap: " + lapCount + "/3";

        if (lapCount >= 4)
        {
            hasFinished1 = true;
        }
	}
}
