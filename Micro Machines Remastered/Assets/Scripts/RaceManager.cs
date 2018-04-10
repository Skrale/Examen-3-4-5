using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RaceManager : MonoBehaviour {

    public Text raceTime1;
    public Text raceTime2;
    public Text lapCounter1;
    public Text lapCounter2;
    public Text pos1;
    public Text pos2;
    bool hasFinished1 = false;
    bool hasFinished2 = false;

    public int lapCount;
    public int lapCount2;
    float secondsCount;
    int minuteCount;

    public bool canFinish = false;
    public bool canFinish2 = false;
    public CarController counter1;
    public CarController2 counter2;
    public GameObject goBack;
    

	void Start ()
    {
        goBack.SetActive(false);
        lapCount++;
        lapCount2++;
	}


    void Update()
    {
        if (counter1.checkpointCounter > counter2.checkpointCounter2)
        {
            pos1.text = "1st";
            pos2.text = "2nd";
        }else
        {
                pos1.text = "2nd";
                pos2.text = "1st";
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }

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
        
            if (hasFinished1 && hasFinished2)
            {
                goBack.SetActive(true);
                Time.timeScale = 0;
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

        if (hasFinished1 && counter1.checkpointCounter > counter2.checkpointCounter2)
        {
            counter1.checkpointCounter = 1000;
            lapCounter1.text = "Lap: 3/3";
            pos1.text = "WINNER";
        }

        if(hasFinished2 && counter2.checkpointCounter2 > counter1.checkpointCounter)
        {
            counter2.checkpointCounter2 = 1000;
            lapCounter2.text = "Lap: 3/3";
            pos2.text = "WINNER";
        }
	}
}
