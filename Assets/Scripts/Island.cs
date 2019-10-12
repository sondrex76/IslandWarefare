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

    //Size is always 2^x + 1, and 33 might just be the best size
    protected int size = 33;
    private float[,] dataArray;
    private GameObject terrain;
    private TerrainData _terrainData;

    public Island(int x, int z, int islandID, List<Texture2D> textures)
    {

        ID = islandID;
        xCord = x;
        zCord = z;
        IslandsNextTo = 0;

        //Always starts with no island next to it
        northIsland = -1;
        eastIsland = -1;
        southIsland = -1;
        westIsland = -1;

        //algorithm for placing the tiles

        xOffSet = Random.Range(-20, 20);
        zOffSet = Random.Range(-20, 20);

        _terrainData = new TerrainData();
        DiamondSquare();
        _terrainData.size = new Vector3(size, 30, size);
        _terrainData.heightmapResolution = size - 1;
        //_terrainData.baseMapResolution = 64;
        //_terrainData.SetDetailResolution(64, 2);
        _terrainData.SetHeights(0, 0, dataArray);

        TerrainLayer[] layer = new TerrainLayer[textures.Count];

        for (int i = 0; i < textures.Count; i++)
        {

            layer[i] = new TerrainLayer();
            layer[i].normalMapTexture = textures[i];
            layer[i].diffuseTexture = textures[i];
            layer[i].tileSize = new Vector2(1, 1);
        }

        _terrainData.terrainLayers = layer;

    }

    public void StartRender()
    {
        AssignSplatMap();
        terrain = Terrain.CreateTerrainGameObject(_terrainData);
        terrain.transform.position = new Vector3(xCord * 75 + xOffSet, 0, zCord * 75 + xOffSet);
    }

    public void EndRender()
    {
        Destroy(terrain.gameObject);
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

    private void DiamondSquare()
    {
        //Declare array
        dataArray = new float[size, size];

        //Set corners
        dataArray[0, 0] = -1;
        dataArray[size - 1, 0] = -1;
        dataArray[0, size - 1] = -1;
        dataArray[size - 1, size - 1] = -1;
        dataArray[size / 2, size / 2] = 1;

        float h = 0.5f;

        float val;
        float rnd;

        for (int i = size - 1; i >= 2; i /= 2)
        {
            int halfSide = i / 2;

            //Square values
            for (int x = 0; x < size - 1; x += i)
            {
                for (int y = 0; y < size - 1; y += i)
                {
                    val = dataArray[x, y];
                    val += dataArray[x + i, y];
                    val += dataArray[x, y + i];
                    val += dataArray[x + i, y + i];

                    val /= 4;

                    rnd = (Random.value * 2.0f * h) - h;
                    val = Mathf.Clamp01(val + rnd);

                    dataArray[x + halfSide, y + halfSide] = val;
                }
            }


            //Diamond values
            for (int x = 0; x < size - 1; x += halfSide)
            {
                for (int y = (x + halfSide) % i; y < size - 1; y += i)
                {
                    val = dataArray[(x - halfSide + size - 1) % (size - 1), y];
                    val += dataArray[(x + halfSide) % (size - 1), y];
                    val += dataArray[x, (y + halfSide) % (size - 1)];
                    val += dataArray[x, (y - halfSide + size - 1) % (size - 1)];

                    val /= 4;

                    rnd = (Random.value * 2 * h) - h;
                    val = Mathf.Clamp01(val + rnd);

                    dataArray[x, y] = val;

                    if (x == 0) dataArray[size - 1, y] = val;
                    if (y == 0) dataArray[x, size - 1] = val;
                }
            }

            h /= 2;
        }

    }
}
