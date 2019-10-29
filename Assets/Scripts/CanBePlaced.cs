
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanBePlaced : MonoBehaviour
{
    void OnCollisionStay(Collision collision)
     {
        transform.parent.GetComponent<ObjectPlacer>().CollisionDetected(this);
        
     }

    void OnCollisionExit(Collision collision)
     {
        transform.parent.GetComponent<ObjectPlacer>().CollisionExit(this);
        
    
     }

}
