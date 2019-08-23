using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    public GameObject objectToPlace;

    public Camera camera;

    int layerMask = 1 << 9;

    Quaternion m_MyQuaternion;

    float yRotation;

    bool canBePlaced = true;

    // Start is called before the first frame update
    void Start()
    {
       layerMask = ~layerMask;
       objectToPlace = Instantiate(objectToPlace, new Vector3(0,0,0), Quaternion.identity);
       objectToPlace.layer = 9;
       objectToPlace.transform.parent = this.transform;

       m_MyQuaternion = new Quaternion();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)){
            objectToPlace.transform.position = hit.point;
            Vector3 up = hit.normal;
            m_MyQuaternion.SetFromToRotation(Vector3.up, up);


            objectToPlace.transform.rotation = m_MyQuaternion;
            yRotation += Input.mouseScrollDelta.y;
            objectToPlace.transform.Rotate(Vector3.up, yRotation * 10f);

        }

        if(Input.GetButtonDown("Fire1") && canBePlaced){
           GameObject newObject = Instantiate(objectToPlace, objectToPlace.transform.position, objectToPlace.transform.rotation);
           Destroy(newObject.GetComponent<CanBePlaced>());
        }
    }

    public void CollisionDetected(CanBePlaced childScript){
        canBePlaced = false;
        Debug.Log("WFADGREDG");
    }

    public void CollisionExit(CanBePlaced childScript){
        canBePlaced = true;
    }


}
