
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanBePlaced : MonoBehaviour
{
    void OnCollisionStay(Collision collision)
     {
        if (collision.transform.tag == "Building" || collision.transform.tag == "Road")
        {
            transform.parent.GetComponent<ObjectPlacer>().CollisionDetected(this);
            Debug.Log("REEE");
        }
     }

    void OnCollisionExit(Collision collision)
     {
        if (collision.collider.tag == "Building" || collision.collider.tag == "Road")
        {
            transform.parent.GetComponent<ObjectPlacer>().CollisionExit(this);
            Debug.Log("REEE");
        }
    }

}
