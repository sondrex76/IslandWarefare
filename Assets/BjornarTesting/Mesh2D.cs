using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Mesh2D : ScriptableObject{ 


    [System.Serializable]
    public class Vertex{
    public Vector2 point;
    public Vector2 normal;
    public float[] u; // UV but only U not V :monkaHMMM:
    }
 
    public Vertex[] vertices;
     
    public int[] lineIndices;
    
    public int VertexCount(){ return vertices.Length;}
    public int LineCount(){ return lineIndices.Length;}


}
