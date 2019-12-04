// save for resource objects
using UnityEngine;

// Basic data for building needed for reconstructing them when saving & loading
[System.Serializable]
public class FactorySave: BuildingSave
{
    public bool isWorking;
    public float remainingTime;
    public float timeRound;
    public int index;
    public int remainingRounds;
    public int originalRounds;

    public FactorySave(bool active, float t, float tRound, int i, int rounds, int startRounds, float health, float yPos, bool finished, Vector3 position, Vector3 rotation) : 
        base(health, yPos, finished, position, rotation)
    {
        isWorking = active;
        remainingTime = t;
        timeRound = tRound;
        index = i;
        remainingRounds = rounds;
        originalRounds = startRounds;
    }
}
