using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Basic data for buildings when saved needed to reconstruct them
public class BuildingSave : WorldObjectSave
{
    public float presentHealth;
    public float yOffset;
    public bool buildingFinished;
}
