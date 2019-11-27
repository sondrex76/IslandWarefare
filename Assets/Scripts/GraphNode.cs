using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphNode : MonoBehaviour
{
    [SerializeField]
    public List<GraphNode> Adjacent;
    public enum Attribute
    {
        Road,
        Residential,
        Commnerical,
        Industrial,
        Office,
        Millitary,

    }
    public Attribute _attribute;

    [SerializeField]
    private string _id;

    [Range(0, 10)]
    public int ExampleInteger;

    private void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        foreach (var node in Adjacent)
        {
            Debug.DrawLine(node.transform.position, transform.position, Color.red);
        }
    }
}
