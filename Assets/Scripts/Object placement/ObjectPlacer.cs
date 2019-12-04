using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    public GameObject objectToPlace;
    public GameObject objectToPlaceTemp;
    public Camera camera;

    public float maxSlope = 20;

    int layerMask = 1 << 9;

    Quaternion m_MyQuaternion;

    float yRotation;

    bool canBePlaced = true;

    // Start is called before the first frame update
    void Start()
    {
        layerMask = ~layerMask;
        objectToPlaceTemp = GameObject.CreatePrimitive(PrimitiveType.Cube);
        objectToPlaceTemp.layer = 9;
        objectToPlaceTemp.transform.parent = this.transform;
        objectToPlaceTemp.AddComponent<CanBePlaced>();

        m_MyQuaternion = new Quaternion();
        this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)){
            objectToPlaceTemp.transform.position = hit.point;
            Vector3 up = hit.normal;
            m_MyQuaternion.SetFromToRotation(Vector3.up, up);


            float angle = Vector3.Angle(hit.normal, Vector3.up);

            objectToPlaceTemp.transform.rotation = m_MyQuaternion;
            yRotation += Input.mouseScrollDelta.y;
            objectToPlaceTemp.transform.Rotate(Vector3.up, yRotation * 10f);

            //objectToPlaceTemp.SetActive(true);

             if(Input.GetButtonDown("Fire1") && canBePlaced && angle < maxSlope){
                objectToPlaceTemp.SetActive(false);
                objectToPlace.SetActive(true);
                GameObject newObject = Instantiate(objectToPlace, objectToPlaceTemp.transform.position, objectToPlaceTemp.transform.rotation);
                Destroy(newObject.GetComponent<CanBePlaced>());
                this.enabled = false;
             }
        } 

      
    }

    public void CollisionDetected(CanBePlaced childScript){
        canBePlaced = false;
    }

    public void CollisionExit(CanBePlaced childScript){
        canBePlaced = true;
    }


}
