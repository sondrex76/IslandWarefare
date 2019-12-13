using UnityEngine;

public class RoadStruct : MonoBehaviour
{

    public Vector3 roadStart;
    public Vector3 roadEnd;
    public Vector3 controllNode1, controllNode2;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshCollider>().convex = false;
        GetComponent<MeshCollider>().isTrigger = false;
        gameObject.layer = 10;
    }

    //Returns the 4 points that are needed to recreate the road
    public RoadSave ReturnRoadSave()
    {
        return new RoadSave(roadStart, controllNode1, controllNode2, roadEnd);
    }
}
