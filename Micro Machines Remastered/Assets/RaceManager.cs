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
    bool hasFinished2 = false;

    public int lapCount;
    public int lapCount2;
    float secondsCount;
    int minuteCount;

    public bool canFinish = false;
    public bool canFinish2 = false;
    

	void Start ()
    {
        lapCount++;
        lapCount2++;
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

        if (!hasFinished2)
        {
            secondsCount += Time.deltaTime;
            raceTime2.text = minuteCount + "m - " + (int)secondsCount + "s ";
            if(secondsCount >= 60)
            {
                minuteCount++;
                secondsCount = 0;
            }
        }

        lapCounter2.text = "Lap: " + lapCount2 + "/3";

        lapCounter1.text = "Lap: " + lapCount + "/3";

        if (lapCount >= 4)
        {
            hasFinished1 = true;
        }

        if(lapCount2 >= 4)
        {
            hasFinished2 = true;
        }
	}
}
