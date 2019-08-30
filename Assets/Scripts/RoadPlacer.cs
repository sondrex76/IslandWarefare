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

    [Range(0,1)]
    public float tTest;
    

   


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


    Mesh mesh;
    [Range(2,32)]
    public int edgeRingCount = 8;
    void GenerateMesh(){

        mesh.Clear();

        //Verts
        List<Vector3> verts = new List<Vector3>();
        for(int ring = 0; ring < edgeRingCount; ring++){
            float t = ring / (edgeRingCount - 1f);
            OrientedPoint op = GetPoint(points, t);
             for(int i = 0; i < shape2D.VertexCount(); i++){
                verts.Add(op.LocalToWorldPos(shape2D.vertices[i].point));
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
        mesh.SetTriangles(triangleIndices, 0);
    }

    private void Awake() {
        mesh = new Mesh();
        mesh.name = "Segment";
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }


    private void Update() {
        GenerateMesh();
}
    public Vector3[] points = new Vector3[4];
    private void OnDrawGizmos() {



        points[0] = new Vector3(19,15,1);
        points[1] = new Vector3(12,15,-17);
        points[2] = new Vector3(40, 17, 2);
        points[3] = new Vector3(47,50,-17);

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

   /* public void Extrude(Mesh mesh, ExtrudeShape shape, OrientedPoint[] path){
        int vertsInShape = shape.verts.Length;
        int segments = path.Length - 1;
        int edgeLoops = path.Length;
        int vertCount = vertsInShape * edgeLoops;
        int triCount = shape.lines.Length * segments;
        int triIndexCount = triCount * 3;

        int[] triangleIndices = new int[triIndexCount];
        Vector3[] vertices = new Vector3[vertCount];
        Vector3[] normals = new Vector3[vertCount];
        Vector2[] uvs = new Vector2[vertCount];

        /*Generation Code 
        for(int i = 0; i < path.Length; i++){
            int offset = i * vertsInShape;
            for(int j = 0; j < vertsInShape; j++){
                int id = offset+j;
                vertices[id] = path[i].LocalToWorld(shape.verts[j].point);
                normals[id] = path.LocalToWorld(shape.verts[j].uvs, i / ((float)edgeLoops));
            }
        }
        



        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangleIndices;
        mesh.normals = normals;
        mesh.uv = uvs;

}*/
    
    
}



