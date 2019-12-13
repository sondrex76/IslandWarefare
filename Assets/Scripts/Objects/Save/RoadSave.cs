using UnityEngine;

[System.Serializable]
public class RoadSave
{

    public float startPos_X, startPos_Y, startPos_Z;
    public float controllNode1_X, controllNode1_Y, controllNode1_Z;
    public float controllNode2_X, controllNode2_Y, controllNode2_Z;
    public float endPos_X, endPos_Y, endPos_Z;


    //I've been told you could just save straight up vector3, but last time I did it I didn't
    //TODO: Use Vector3
    public RoadSave(Vector3 startPos, Vector3 controllNode1, Vector3 controllNode2, Vector3 endPos)
    {
        startPos_X = startPos.x;
        startPos_Y = startPos.y;
        startPos_Z = startPos.z;

        controllNode1_X = controllNode1.x;
        controllNode1_Y = controllNode1.y;
        controllNode1_Z = controllNode1.z;

        controllNode2_X = controllNode2.x;
        controllNode2_Y = controllNode2.y;
        controllNode2_Z = controllNode2.z;

        endPos_X = endPos.x;
        endPos_Y = endPos.y;
        endPos_Z = endPos.z;
    }
}
