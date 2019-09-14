using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;




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



    void GenerateMesh(Vector3[] pts){

        mesh.Clear();

        //Verts
        List<Vector3> verts = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();

        for(int ring = 0; ring < edgeRingCount; ring++){
            float t = ring / (edgeRingCount - 1f);
            OrientedPoint op = GetPoint(pts, t);
             for(int i = 0; i < shape2D.VertexCount(); i++){
                verts.Add(op.LocalToWorldPos(shape2D.vertices[i].point));
                normals.Add(op.LocalToWorldDir(shape2D.vertices[i].normal));

            }
        }

        List<int> triangleIndices = new List<int>();

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
        mesh = new Mesh();
        mesh.name = "Segment";
        GetComponent<MeshFilter>().sharedMesh = mesh;
        layerMask = ~layerMask;
    }

    int layerMask = 1 << 9;
    int layerMask2 = 1 << 10;


    Vector3 startPoint;
    Vector3 troughStartAndEnd;


    Vector3 connectingRoadEnd;
    Vector3 connectingroadStart;

    public bool connecting = false;
    private void Update() {

        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)){


            if(Input.GetButtonDown("Jump")){
                if(isPlacing){
                    GameObject road = new GameObject("Road");
                    road.layer = 9;
                    road.AddComponent<MeshFilter>();
                    road.AddComponent<MeshRenderer>();
                    mesh = road.GetComponent<MeshFilter>().mesh;
                    road.GetComponent<MeshRenderer>().material = m_material;
                    road.AddComponent<roadStruct>();
                    road.AddComponent<MeshCollider>();
                    road.GetComponent<roadStruct>().roadStart = pts[0];
                    road.GetComponent<roadStruct>().roadEnd = pts[3];
                    isPlacing = false;
                } else{

                    if(!connecting){
                        Debug.Log(hit.point);
                        startPoint = hit.point;
                        isPlacing = true;
                        pts[0] = new Vector3(hit.point.x, hit.point.y + 0.2f, hit.point.z);
                        pts[2] = hit.point;
                    } else
                    {
                        isPlacing = true;
                        pts[0] = connectingRoadEnd;
                        pts[1] = transform.forward * 4;
                        pts[2] = new Vector3(pts[1].x, pts[1].y, pts[1].z); ;
                    }
                }
        }

     

        if(isPlacing){
            if(!connecting){
                    //pts[1] = startPoint + -Vector3.forward * 50;
                    pts[1] = pts[3];

                }
                pts[3] = new Vector3(hit.point.x, hit.point.y + 0.2f, hit.point.z);
                GenerateMesh(pts);
            }

        if(Input.GetMouseButtonDown(2)){
            mesh.Clear();
            isPlacing = false;
            }

           
        }

        RaycastHit hit2;
        Ray ray2 = camera.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray2, out hit2, Mathf.Infinity, layerMask2)){

            if (hit2.collider.gameObject.name == "Road"){

                Debug.Log("YES THIS IS HITTING A ROAD");


                connectingRoadEnd = hit2.collider.gameObject.GetComponent<roadStruct>().roadEnd;
                connectingroadStart = hit2.collider.gameObject.GetComponent<roadStruct>().roadStart;

               
                connecting = true;
        } else {
                connecting = false;
                Debug.Log("NOt hitting road");
            }
        } else if(!isPlacing)
        {
            connecting = false;
        }

    }

    [Range(0,1)]
    public float tTest;

    public Vector3[] points = new Vector3[4];
    private void OnDrawGizmos() {



        points[0] = new Vector3(19,15,1);
        points[1] = new Vector3(12,15,-17);
        points[2] = new Vector3(40, 17, 2);
        points[3] = new Vector3(47,50,-17);

        Gizmos.DrawSphere(pts[1], 2);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(pts[2], 1);
        Gizmos.color = Color.white;

        Debug.Log("Point1" + pts[1]);


        drawMesh(points);
    }

    public void drawMesh(Vector3[] points){

    OrientedPoint testPoint = GetPoint(points, tTest);

    Vector3[] verts = shape2D.vertices.Select(v => testPoint.LocalToWorldPos(v.point)).ToArray();


    void DrawPoint(Vector2 localPos){
            Gizmos.DrawSphere(testPoint.LocalToWorldPos(localPos * 2f), 0.3f);


        }


        Handles.PositionHandle(testPoint.position, testPoint.rotation);


       for (int i = 0; i < shape2D.lineIndices.Length-1; i++)
        {
           Vector3 a = verts[shape2D.lineIndices[i]];
           Vector3 b = verts[shape2D.lineIndices[i+1]];

           Gizmos.DrawLine(a,b);

        }
    }

    
}



