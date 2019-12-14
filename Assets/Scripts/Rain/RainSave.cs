using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RainSave {
    public bool isRaining;
    public float nextCycle;
    public float currentCycleTime;

    public RainSave(bool raining, float cycleTime, float currentTime)
    {
        isRaining = raining;
        nextCycle = cycleTime;
        currentCycleTime = currentTime;
    }
}
