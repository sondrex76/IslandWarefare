using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    int currentSpeed = 50;

    // Start is called before the first frame update

    public void UpdateSpeed(int speed)
    {
        if (speed != 0)
        {
            Time.timeScale = currentSpeed = speed;
            GameManager.isPaused = false;
        }
        else
        {
            GameManager.isPaused = true;
        }
    }
}
