
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CanBePlaced : MonoBehaviour
{
    void OnCollisionStay(Collision collision)
     {
        if (collision.transform.tag == "Building" || collision.transform.tag == "Road" || collision.transform.tag == "Factory")
        {
            try
            {
                transform.parent.GetComponent<ObjectPlacer>().CollisionDetected(this);
                Debug.Log(transform.name);
            }
            catch(Exception e)
            {
                Debug.Log(transform.name);
                Debug.Log("Error in OnCollisionStay within canBePlaced");
            }
        }
     }

    void OnCollisionExit(Collision collision)
     {
        if (collision.collider.tag == "Building" || collision.collider.tag == "Road" || collision.transform.tag == "Factory")
        {
            try
            {
                transform.parent.GetComponent<ObjectPlacer>().CollisionExit(this);
                Debug.Log("REEE");
            }
            catch(Exception e)
            {
                Debug.Log(transform.name);
                Debug.Log("Error in OnCollisionExit within canBePlaced");
            }
        }
    }

}
