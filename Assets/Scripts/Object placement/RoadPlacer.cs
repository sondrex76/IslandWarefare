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
    // Start is called before the first frame updateMesh2D shape2D;
    [SerializeField]
    Mesh2D shape2D;
    Mesh mesh;
    [Range(2,32)]
    public int edgeRingCount = 8;
    Vector3[] pts = new Vector3[4];
    bool isPlacing;
    Vector3 root;
    public Camera camera;
    public Material m_material;

    int layerMask = 1 << 9;
    int layerMask2 = 1 << 10;


    Vector3 startPoint;
    Vector3 troughStartAndEnd;


    Vector3 connectingRoadEnd;
    Vector3 connectingroadStart;
    Vector3 roadMiddlePoint;

    int roadCounter = 0;

    public bool connecting = false;
    bool straight;

    [SerializeField]
    float yOffset;


    OrientedPoint GetPoint(Vector3[] pts, float t){
        float omt = 1f-t;
        float omt2 = omt * omt;
        float t2 = t*t;

        Vector3 pos = pts[0] *  (omt2 * omt)     +
                pts[1] * (3f * omt2 * t)         +
                pts[2] * (3f * omt * t2)         +
                pts[3] * (t2 * t);


        Vector3 tangent = GetTangent(pts, t);

        return new OrientedPoint(pos, tangent);
    }

    Vector3 GetTangent(Vector3[] pts, float t){
        float omt = 1f-t;
        float omt2 = omt * omt;
        float t2 = t*t;


         return pts[0] *  (-omt2)                +
               pts[1] * (3f * omt2  - 2 * omt)   +
               pts[2] * (-3f * t2 + 2 * t)       +
               pts[3] * (t2);
    }

    Vector3 GetNormal(Vector3[] pts, float t, Vector3 up){
        Vector3 tng = GetTangent(pts, t);
        Vector3 binormal = Vector3.Cross(up, tng).normalized;
        return Vector3.Cross(tng, binormal);
    }

    Quaternion GetOrientation(Vector3[] pts, float t, Vector3 up){
        Vector3 tng = GetTangent(pts, t);
        Vector3 nrm = GetNormal(pts ,t, up);
        return Quaternion.LookRotation(tng, nrm);
    }

    List<Vector3> verts;
    List<Vector3> normals;
    List<int> triangleIndices;
    List<Vector3> graphhNodesPos;

    public GameObject graph;


    void GenerateMesh(Vector3[] pts){

        mesh.Clear();

        //Verts
        verts = new List<Vector3>();
        normals = new List<Vector3>();
        graphhNodesPos = new List<Vector3>();


        for (int ring = 0; ring < edgeRingCount; ring++){
            float t = ring / (edgeRingCount - 1f);
            OrientedPoint op = GetPoint(pts, t);

            graphhNodesPos.Add(op.position);


             for (int i = 0; i < shape2D.VertexCount(); i++){
                verts.Add(op.LocalToWorldPos(shape2D.vertices[i].point));
                normals.Add(op.LocalToWorldDir(shape2D.vertices[i].normal));

            }
        }

        triangleIndices = new List<int>();

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

        
        Debug.Log("Placing new road");

    }

    GameObject roadTemp;

    private void Update() {

        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)){


            if(Input.GetMouseButtonDown(0)){
                if(isPlacing){
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
                    road.GetComponent<roadStruct>().pivotPoint = pts[2];
                    road.tag = "Road";
                    Debug.Log("Placing new road");
                    isPlacing = false;


                    foreach (Vector3 nodePos in graphhNodesPos)
                    {
                        GameObject node = new GameObject();
                        node.transform.position = new Vector3(nodePos.x, Mathf.Floor(nodePos.y), nodePos.z);
                        node.transform.parent = graph.transform;
                        node.AddComponent<GraphNode>();
                        node.GetComponent<GraphNode>()._attribute = GraphNode.Attribute.Road;
                        node.GetComponent<GraphNode>().Adjacent = new List<GraphNode>();
                    }

                    Instantiate(road);
                    Destroy(road);
                    roadCounter++;
                    straight = false;
                    graph.GetComponent<Graph>().AddNodes();

                }



                else {

                    if (!connecting){
                        startPoint = hit.point;
                        isPlacing = true;
                        pts[0] = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                        pts[2] = hit.point;
                        straight = true;
                        Debug.Log("Starting placement of road");
                    } else
                    {

                        float distanceToStart = Vector3.Distance(hit.point, connectingroadStart);
                        float distanceToEnd = Vector3.Distance(hit.point, connectingRoadEnd);

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

            if (isPlacing)
            {
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
               
              
                if(hit.collider.gameObject.tag == "Road")
                {

                    roadStruct connectingRoad = hit.collider.gameObject.GetComponent<roadStruct>();

                    float distanceToStart = Vector3.Distance(hit.point, connectingRoad.roadStart);
                    float distanceToEnd = Vector3.Distance(hit.point, connectingRoad.roadEnd);

                    if(distanceToStart < 5f)
                    {
                        Vector3 vectorThoughConnectingRoad = connectingRoad.roadEnd - connectingRoad.pivotPoint;

                        pts[2] = connectingRoad.roadStart - vectorThoughConnectingRoad * 0.1f;
                        pts[3] = connectingRoad.roadStart;
                    }
                    else if(distanceToEnd < 5f)
                    {
                        Vector3 vectorThoughConnectingRoad = connectingRoad.roadEnd - connectingRoad.pivotPoint;

                        pts[2] = connectingRoad.roadEnd + vectorThoughConnectingRoad * 0.1f;
                        pts[3] = connectingRoad.roadEnd;
                    }


             
                } else
                {
                    
                    if (!connecting)
                    {
                        pts[2] = startPoint;
                        pts[1] = hit.point;
                    }else 
                    {
                        pts[2] = pts[1];
                    }

                    pts[3] = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                }

                Debug.Log(connecting);

                GenerateMesh(pts);


            }

            if (Input.GetMouseButtonDown(2)){
                mesh.Clear();
                isPlacing = false;
                connecting = false;
                straight = false;
            }

           
        }

        RaycastHit hit2;
        Ray ray2 = camera.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray2, out hit2, Mathf.Infinity, layerMask2)){

            Debug.Log(hit2.collider.tag);

            if (hit2.collider.gameObject.tag == "Road" && !straight){

                connectingRoadEnd = hit2.collider.gameObject.GetComponent<roadStruct>().roadEnd;
                connectingroadStart = hit2.collider.gameObject.GetComponent<roadStruct>().roadStart;
                roadMiddlePoint = hit2.collider.gameObject.GetComponent<roadStruct>().pivotPoint;

               
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

    
}



