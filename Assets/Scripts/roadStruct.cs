using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roadStruct : MonoBehaviour
{

    public Vector3 roadStart;
    public Vector3 roadEnd;
    public Vector3 pivotPoint;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshCollider>().convex = true;
        GetComponent<MeshCollider>().isTrigger = true;
        gameObject.layer = 10;
    }

    // Update is called once per frame
    void Update()
    {


    }
}
