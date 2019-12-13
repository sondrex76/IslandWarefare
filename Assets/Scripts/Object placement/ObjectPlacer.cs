using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    public GameObject objectToPlace;
    public GameObject objectToPlaceTemp;
    public GameManager gameManager;
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
        gameManager = FindObjectOfType<GameManager>();
        m_MyQuaternion = new Quaternion();
        this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Send ray from camera to mouseposition 
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        //Check if we hit something that's not a building
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)){
            //Set position to the place the ray hit
            objectToPlaceTemp.transform.position = hit.point;
            //Make the building be straight with terrain
            Vector3 up = hit.normal;
            m_MyQuaternion.SetFromToRotation(Vector3.up, up);
            float angle = Vector3.Angle(hit.normal, Vector3.up);
            objectToPlaceTemp.transform.rotation = m_MyQuaternion;

            //Roatate building with mouse scroll
            yRotation += Input.mouseScrollDelta.y;
            objectToPlaceTemp.transform.Rotate(Vector3.up, yRotation * 10f);

            //Make the temp a child of this object
            objectToPlaceTemp.transform.parent = this.transform;

            //Place the building if if building is not in another and angle is within range
             if(Input.GetButtonDown("Fire1") && canBePlaced && angle < maxSlope){
                //Destroy the temp object
                Destroy(objectToPlaceTemp);
                //Activate and place building and subtract money
                objectToPlace.SetActive(true);
                AbstractBuilding building = objectToPlace.GetComponent<AbstractBuilding>();
                gameManager.BuyBuilding(building.price);
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
