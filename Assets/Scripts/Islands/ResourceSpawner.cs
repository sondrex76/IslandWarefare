using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> resources;

    private Terrain terrain;

    [SerializeField]
    private int seperation = 6;

    // Start is called before the first frame update
    public void GenerateResources()
    {
        terrain = FindObjectOfType<Terrain>();

        int x = terrain.terrainData.heightmapWidth;
        int y = terrain.terrainData.heightmapHeight;

        float[,] map = terrain.terrainData.GetHeights(0, 0, x, y);
        float scaleX = terrain.terrainData.size.x / x;
        float scaleZ = terrain.terrainData.size.z / y;

        Debug.Log(Const.size + " " + terrain.terrainData.size.y);
        Debug.Log(scaleX);

        for (int i = 0; i < x; i += seperation)
        {
            for (int j = 0; j < y; j += seperation)
            {
                if (map[i, j] * Const.islandHeight > 14.3f)
                {
                    int numb = Random.Range(0, resources.Count);
                    Vector3 position = new Vector3(terrain.transform.position.x + j * scaleX, map[i, j] * Const.islandHeight, terrain.transform.position.z +  i * scaleZ);
                    Instantiate(resources[numb], position, transform.rotation, transform);
                    Debug.Log("Wat");
                }
            }
        }
    }
}
