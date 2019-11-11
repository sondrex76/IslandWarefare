using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuIsland : MonoBehaviour
{
    protected int size = 127;
    protected int seed = 185;
    protected float gradient = 25;

    [SerializeField]
    protected Material material;

    // Start is called before the first frame update
    void Start()
    {
        float[,] dataArray = PerlinNoise.GetPerlinNoise(size, size, seed, gradient);

        GameObject terrain;

        TerrainData data = new TerrainData();
        data.size = new Vector3(size, 100, size);
        data.heightmapResolution = size - 1;
        data.SetHeights(0, 0, dataArray);
        terrain = Terrain.CreateTerrainGameObject(data);
        terrain.GetComponent<Terrain>().materialTemplate = material;
        terrain.transform.position = new Vector3(-size / 2, -0.1f, -800);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
