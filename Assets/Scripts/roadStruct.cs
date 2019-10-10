using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roadStruct : MonoBehaviour
{

    public Vector3 roadStart;
    public Vector3 roadEnd;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshCollider>().convex = true;
        gameObject.layer = 10;
    }

    // Update is called once per frame
    void Update()
    {


    }
}
