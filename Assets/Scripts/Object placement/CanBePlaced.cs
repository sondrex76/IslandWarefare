using System;
using UnityEngine;

public class CanBePlaced : MonoBehaviour
{
    //Check if we collide with a bulding and make it impossible to place a building
    void OnCollisionStay(Collision collision)
     {
        if (collision.transform.tag == "Building" || collision.transform.tag == "Road" 
            || collision.transform.tag == "Factory" || collision.transform.tag == "Harvester")
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

    //Check of object exit a collider and make it possible to place
    void OnCollisionExit(Collision collision)
     {
        if (collision.collider.tag == "Building" || collision.collider.tag == "Road" 
            || collision.transform.tag == "Factory" || collision.transform.tag == "Harvester")
        {
            try
            {
                transform.parent.GetComponent<ObjectPlacer>().CollisionExit(this);
                //We use only the highest standard of debugging
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
