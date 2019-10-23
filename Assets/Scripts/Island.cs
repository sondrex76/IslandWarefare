using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Island : MonoBehaviour
{
    public int ID;
    public int xCord;
    public int zCord;
    public int IslandsNextTo;

    protected int xOffSet;
    protected int zOffSet;

    //Keeps info on whether there is an island next to it or not
    public int northIsland;
    public int eastIsland;
    public int southIsland;
    public int westIsland;

    protected Material _material;

    //Size is always 2^x + 1, and 33 might just be the best size
    protected int size = 129;
    private float[,] dataArray;
    private GameObject terrain;
    private TerrainData _terrainData;

    public Island(int x, int z, int islandID, Material material)
    {

        ID = islandID;
        xCord = x;
        zCord = z;
        IslandsNextTo = 0;
        _material = material;

        //Always starts with no island next to it
        northIsland = -1;
        eastIsland = -1;
        southIsland = -1;
        westIsland = -1;

        //algorithm for placing the tiles

        xOffSet = Random.Range(-20, 20);
        zOffSet = Random.Range(-20, 20);

        PerlinNoise noise = new PerlinNoise();

        _terrainData = new TerrainData();
        //DiamondSquare();
        dataArray = noise.GetPerlinNoise(size, size, ID);
        _terrainData.size = new Vector3(size, 100, size);
        _terrainData.heightmapResolution = size - 1;
        //_terrainData.baseMapResolution = 64;
        //_terrainData.SetDetailResolution(64, 2);
        _terrainData.SetHeights(0, 0, dataArray);

        /*
        TerrainLayer[] layer = new TerrainLayer[textures.Count];

        for (int i = 0; i < textures.Count; i++)
        {

            layer[i] = new TerrainLayer();
            layer[i].normalMapTexture = textures[i];
            layer[i].diffuseTexture = textures[i];
            layer[i].tileSize = new Vector2(1, 1);
        }

        _terrainData.terrainLayers = layer;
        */

    }

    public void StartRender()
    {
        //AssignSplatMap();
        terrain = Terrain.CreateTerrainGameObject(_terrainData);
        terrain.AddComponent<MeshRenderer>();
        terrain.GetComponent<MeshRenderer>().material = _material;
        terrain.transform.position = new Vector3(xCord * 350 + xOffSet, -0.1f, zCord * 350 + xOffSet);
    }

    public void EndRender()
    {
        Destroy(terrain);
    }

    private void AssignSplatMap()
    {
        float[,,] splatmapData = new float[_terrainData.alphamapWidth, _terrainData.alphamapHeight, _terrainData.alphamapLayers];

        for (int y = 0; y < _terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < _terrainData.alphamapWidth; x++)
            {
                
                    splatmapData[x, y, 0] = 1;
            }
        }

        _terrainData.SetAlphamaps(0, 0, splatmapData);
    }
    
}
