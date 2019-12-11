using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;


/*
If you read this I must apologize for the horrendous crimes against every possible coding standard I've done here
*/


public struct OrientedPoint {
    public Vector3 position;
    public Quaternion rotation;

    public OrientedPoint(Vector3 position, Vector3 forward){
        this.rotation = Quaternion.LookRotation(forward);
        this.position = position;
    }


     public OrientedPoint(Vector3 position, Quaternion rotation){
        this.rotation = rotation;
        this.position = position;
    }

    public Vector3 WorldToLocal(Vector3 point){
        return Quaternion.Inverse(rotation) * (point - position);
    }

    public Vector3 LocalToWorldDir(Vector3 dir){
        return rotation * dir;
    }

    public Vector3 LocalToWorldPos(Vector3 localPos){
        return position + rotation * localPos;
    }

}



public class RoadPlacer : MonoBehaviour
{
    //The 2D slice we use to construct the road
    [SerializeField]
    Mesh2D shape2D;
    Mesh mesh;
    //Amount of segments the roads will be split into to make the road curve
    [Range(2,128)]
    public int edgeRingCount = 8;
    //Points for setting start of road and the two controllpoints used in the bezier curve
    Vector3[] pts = new Vector3[4];
    //Check for if we are currently placing the end of the road
    bool isPlacing;
    Vector3 root;
    public Camera camera;
    public Material m_material;

    //Layermask to make sure raycast ignore the temp road to visualize
    int layerMask = 1 << 9;
    int layerMask2 = 1 << 10;


    Vector3 startPoint;
    //End of road we are connecting to
    Vector3 connectingRoadEnd;
    //Start of road we are connecting to
    Vector3 connectingroadStart;
    //Middle of road we are connecting to
    Vector3 roadMiddlePoint;

    //Just used for naming the roads for easier debugging
    int roadCounter = 0;
    //Check for if we are connecting to another road
    public bool connecting = false;
    //Check if the current road we are making is straight
    bool straight;

    //To make sure the road is over the ground and not in it
    [SerializeField]
    float yOffset;



    //List of vertices used to construct the road
    List<Vector3> verts;
    //List of the normal of the road
    List<Vector3> normals;
    //List of indices to connect the vertices
    List<int> triangleIndices;
    //List of the position to the graphnodes that the road is going to create
    List<Vector3> graphhNodesPos;

    public GameObject graph;

    //Get the a point that corresponds to t on the bezier curve
    OrientedPoint GetPoint(Vector3[] pts, float t)
    {
        float omt = 1f - t;
        float omt2 = omt * omt;
        float t2 = t * t;

        Vector3 pos = pts[0] * (omt2 * omt) +
                pts[1] * (3f * omt2 * t) +
                pts[2] * (3f * omt * t2) +
                pts[3] * (t2 * t);


        Vector3 tangent = GetTangent(pts, t);

        return new OrientedPoint(pos, tangent);
    }

    Vector3 GetTangent(Vector3[] pts, float t)
    {
        float omt = 1f - t;
        float omt2 = omt * omt;
        float t2 = t * t;


        return pts[0] * (-omt2) +
              pts[1] * (3f * omt2 - 2 * omt) +
              pts[2] * (-3f * t2 + 2 * t) +
              pts[3] * (t2);
    }


    //Generate the mesh of the road
    void GenerateMesh(Vector3[] pts){

        //Clear the mesh for good measure
        mesh.Clear();

        //Verts, normals and graph nodes
        verts = new List<Vector3>();
        normals = new List<Vector3>();
        graphhNodesPos = new List<Vector3>();

        //Loop through every segment ring
        for (int ring = 0; ring < edgeRingCount; ring++){
            float t = ring / (edgeRingCount - 1f);
            //Get a point given the points and ring position in segment
            OrientedPoint op = GetPoint(pts, t);


            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(op.position + Vector3.up * 100f, Vector3.down, out hit, Mathf.Infinity, layerMask))
            {
                op.position = hit.point;
            }

            //Add graphnodes at segment position
            graphhNodesPos.Add(op.position);

            //Looping through each vertex in shape2D, adding vertecies and normals on the position that the ring is
             for (int i = 0; i < shape2D.VertexCount(); i++){
                verts.Add(op.LocalToWorldPos(shape2D.vertices[i].point));
                normals.Add(op.LocalToWorldDir(shape2D.vertices[i].normal));

            }
        }


        triangleIndices = new List<int>();

        //Loops through each segment connecting the vertexes
        for(int ring = 0; ring < edgeRingCount - 1; ring++){

            int rootIndex = ring * shape2D.VertexCount();
            int rootIndexNext = (ring+1) * shape2D.VertexCount();

            for (int line = 0; line < shape2D.LineCount(); line+=2){
                
                int lineIndexA = shape2D.lineIndices[line];
                int lineIndexB = shape2D.lineIndices[line+1];
                
                int currentA = rootIndex + lineIndexA;
                int currentB = rootIndex + lineIndexB;

                int nextA = rootIndexNext + lineIndexA;
                int nextB = rootIndexNext + lineIndexB;

                triangleIndices.Add(currentA);
                triangleIndices.Add(nextA);
                triangleIndices.Add(nextB);

                triangleIndices.Add(currentA);
                triangleIndices.Add(nextB);
                triangleIndices.Add(currentB);
            }
        }



        mesh.SetVertices(verts);
        mesh.SetNormals(normals);
        mesh.SetTriangles(triangleIndices, 0);
    }

    private void Awake() {
        layerMask = ~layerMask;
        mesh = new Mesh();


        this.enabled = false;

    }

    GameObject roadTemp;

    private void Update() {

        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)){

            if(Input.GetMouseButtonDown(0)){
                //Placing road when left click is pressed and is placing end
                if (isPlacing){
                    Destroy(roadTemp);
                    GameObject road = new GameObject("Road: " + roadCounter);
                    road.transform.position = new Vector3(road.transform.position.x, road.transform.position.y + yOffset, road.transform.position.z);
                    road.layer = 9;
                    road.AddComponent<MeshFilter>();
                    road.AddComponent<MeshRenderer>();
                    road.GetComponent<MeshFilter>().mesh.SetVertices(verts);
                    road.GetComponent<MeshFilter>().mesh.SetNormals(normals);
                    road.GetComponent<MeshFilter>().mesh.SetTriangles(triangleIndices, 0);
                    road.GetComponent<MeshRenderer>().material = m_material;
                    road.AddComponent<roadStruct>();
                    road.AddComponent<MeshCollider>();
                    road.GetComponent<roadStruct>().roadStart = pts[0];
                    road.GetComponent<roadStruct>().roadEnd = pts[3];
                    road.GetComponent<roadStruct>().controllNode1 = pts[1];
                    road.GetComponent<roadStruct>().controllNode2 = pts[2];
                    road.tag = "Road";
                    Debug.Log("Placing new road");
                    isPlacing = false;

                    //TODO: Make Nodes place based on length of road not segments
                    //Placing graph nodes along road
                    foreach (Vector3 nodePos in graphhNodesPos)
                    {
                        GameObject node = new GameObject();
                        node.transform.position = new Vector3(nodePos.x, Mathf.Floor(nodePos.y), nodePos.z);
                        node.transform.parent = graph.transform;
                        node.AddComponent<GraphNode>();
                        node.GetComponent<GraphNode>().attribute = GraphNode.Attribute.Road;
                        node.GetComponent<GraphNode>().Adjacent = new List<GraphNode>();
                    }
                    //Place road
                    Instantiate(road);
                    //Destory gameobject
                    Destroy(road);
                    roadCounter++;
                    straight = false;
                    //Add nodes to the graph
                    graph.GetComponent<Graph>().AddNodes();
                    //Disable script
                    this.enabled = false;

                }


                //Start placing roads start point
                else {
                    //If we are not connecting to another road place the starting point at raycast hit
                    //and set second controll point to the same so we get a straight line
                    if (!connecting){
                        startPoint = hit.point;
                        isPlacing = true;
                        pts[0] = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                        pts[2] = hit.point;
                        straight = true;
                        Debug.Log("Starting placement of road");
                    } else
                    {
                        //Get the distance to the start and end of the road we are connecting to
                        float distanceToStart = Vector3.Distance(hit.point, connectingroadStart);
                        float distanceToEnd = Vector3.Distance(hit.point, connectingRoadEnd);

                        //If we connect to the start of the road, we set the start point of the new road to the start of the connection.
                        //Find a line through the connecting road and set the controll points on that line 
                        if (distanceToStart < distanceToEnd)
                        {
                            Vector3 vectorThroughRoad = (connectingRoadEnd - roadMiddlePoint);

                            pts[0] = connectingroadStart;
                            pts[1] = connectingroadStart - vectorThroughRoad * 0.1f;
                            pts[2] = new Vector3(pts[1].x, pts[1].y, pts[1].z);
                        }
                        else
                        {

                            Vector3 vectorThroughRoad = (connectingRoadEnd - roadMiddlePoint);

                            pts[0] = connectingRoadEnd;
                            pts[1] = connectingRoadEnd + vectorThroughRoad * 0.1f;
                            pts[2] = new Vector3(pts[1].x, pts[1].y, pts[1].z);
                        }
                        isPlacing = true;
                      
                        
                    }
                }

                connecting = false;
        }
            //Generate a temp road that show how the road is going to look
            if (isPlacing)
            {
                //Destory it each frame
                Destroy(roadTemp);

                roadTemp = new GameObject("Road: " + roadCounter);
                roadTemp.layer = 9;
                roadTemp.AddComponent<MeshFilter>();
                roadTemp.AddComponent<MeshRenderer>();

                roadTemp.GetComponent<MeshFilter>().mesh.SetVertices(verts);
                roadTemp.GetComponent<MeshFilter>().mesh.SetNormals(normals);
                roadTemp.GetComponent<MeshFilter>().mesh.SetTriangles(triangleIndices, 0);

                roadTemp.GetComponent<MeshRenderer>().material = m_material;

            }


            if (isPlacing){

                //If we are connecting to the end/start of a road get the road we are connecting to and the distance to end/start               
                if(hit.collider.gameObject.tag == "Road")
                {

                    roadStruct connectingRoad = hit.collider.gameObject.GetComponent<roadStruct>();

                    float distanceToStart = Vector3.Distance(hit.point, connectingRoad.roadStart);
                    float distanceToEnd = Vector3.Distance(hit.point, connectingRoad.roadEnd);

                    //Set controllpoint2 at the line through the  connecting road, and set the end of the road to the start of the connecting road
                    if(distanceToStart < 5f)
                    {
                        Vector3 vectorThoughConnectingRoad = connectingRoad.roadEnd - connectingRoad.controllNode2;

                        pts[2] = connectingRoad.roadStart - vectorThoughConnectingRoad * 0.1f;
                        pts[3] = connectingRoad.roadStart;
                    }
                    else if(distanceToEnd < 5f)
                    {
                        Vector3 vectorThoughConnectingRoad = connectingRoad.roadEnd - connectingRoad.controllNode2;

                        pts[2] = connectingRoad.roadEnd + vectorThoughConnectingRoad * 0.1f;
                        pts[3] = connectingRoad.roadEnd;
                    }


             
                } else
                {
                    //Set controllpoin2 to start of the road and controllpoint1 to the hit point
                    if (!connecting)
                    {
                        pts[2] = startPoint;
                        pts[1] = hit.point;
                    }else 
                    {
                        pts[2] = pts[1];
                    }
                    //Set the end point to the hit point
                    pts[3] = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                }

                GenerateMesh(pts);


            }


            //Cancle the placement
            if (Input.GetMouseButtonDown(2)){
                mesh.Clear();
                Destroy(roadTemp);
                isPlacing = false;
                connecting = false;
                straight = false;
                this.enabled = false;
            }

           
        }

        RaycastHit hit2;
        Ray ray2 = camera.ScreenPointToRay(Input.mousePosition);
        //When the ray hit a road, get the data from the road and set connecting to true
        if(Physics.Raycast(ray2, out hit2, Mathf.Infinity, layerMask2)){

            Debug.Log(hit2.collider.tag);

            if (hit2.collider.gameObject.tag == "Road" && !straight){

                connectingRoadEnd = hit2.collider.gameObject.GetComponent<roadStruct>().roadEnd;
                connectingroadStart = hit2.collider.gameObject.GetComponent<roadStruct>().roadStart;
                roadMiddlePoint = hit2.collider.gameObject.GetComponent<roadStruct>().controllNode2;

               
                connecting = true;
        } else
            {
                connecting = false;
               }
        } else if(!isPlacing)
        {
            connecting = false;
        }

    }

    [Range(0,1)]
    public float tTest;

    public Vector3[] points = new Vector3[4];

    //Generate the roads from save file
    public void GenerateRoad(Vector3 startPos, Vector3 controllNode1, Vector3 controllNode2, Vector3 endPos)
    {
        mesh = new Mesh();

        pts[0] = startPos;
        pts[1] = controllNode1;
        pts[2] = controllNode2;
        pts[3] = endPos;

        GenerateMesh(pts);

        Destroy(roadTemp);
        GameObject road = new GameObject("Road: " + roadCounter);
        road.transform.position = new Vector3(road.transform.position.x, road.transform.position.y + yOffset, road.transform.position.z);
        road.layer = 9;
        road.AddComponent<MeshFilter>();
        road.AddComponent<MeshRenderer>();
        road.GetComponent<MeshFilter>().mesh.SetVertices(verts);
        road.GetComponent<MeshFilter>().mesh.SetNormals(normals);
        road.GetComponent<MeshFilter>().mesh.SetTriangles(triangleIndices, 0);
        road.GetComponent<MeshRenderer>().material = m_material;
        road.AddComponent<roadStruct>();
        road.AddComponent<MeshCollider>();
        road.GetComponent<roadStruct>().roadStart = pts[0];
        road.GetComponent<roadStruct>().roadEnd = pts[3];
        road.GetComponent<roadStruct>().controllNode1 = pts[1];
        road.GetComponent<roadStruct>().controllNode2 = pts[2];
        road.tag = "Road";
        Debug.Log("Placing new road");
        isPlacing = false;


        foreach (Vector3 nodePos in graphhNodesPos)
        {
            GameObject node = new GameObject();
            node.transform.position = new Vector3(nodePos.x, Mathf.Floor(nodePos.y), nodePos.z);
            node.transform.parent = graph.transform;
            node.AddComponent<GraphNode>();
            node.GetComponent<GraphNode>().attribute = GraphNode.Attribute.Road;
            node.GetComponent<GraphNode>().Adjacent = new List<GraphNode>();
        }

        Instantiate(road);
        Destroy(road);
        roadCounter++;
        straight = false;
        graph.GetComponent<Graph>().AddNodes();


    }


    
}



