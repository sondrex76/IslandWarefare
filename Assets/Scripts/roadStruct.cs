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
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(2)){
            GetComponent<MeshCollider>().convex = true;
            gameObject.layer = 10;
        }
    }
}
