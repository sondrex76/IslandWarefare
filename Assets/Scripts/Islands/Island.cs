using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Island : MonoBehaviour
{
    public int ID;
    public int xCord;
    public int zCord;
    public int IslandsNextTo;

    protected int xOffSet;
    protected int zOffSet;

    public string fileName;

    //Keeps info on whether there is an island next to it or not
    public int northIsland;
    public int eastIsland;
    public int southIsland;
    public int westIsland;

    protected Material _material;

    //Size is always 2^x + 1, and 33 might just be the best size
    private GameObject terrain;

    public Island(int x, int z, int islandID, Material material)
    {

        ID = islandID;
        xCord = x;
        zCord = z;
        IslandsNextTo = 0;
        _material = material;
        fileName = "/island" + ID;

        //Always starts with no island next to it
        northIsland = -1;
        eastIsland = -1;
        southIsland = -1;
        westIsland = -1;

        //algorithm for placing the tiles

        xOffSet = Random.Range(-100, 100);
        zOffSet = Random.Range(-100, 100);

        if (!File.Exists(Application.persistentDataPath + "/island" + ID))
        {
            float[,] dataArray = PerlinNoise.GetPerlinNoise(Const.size, Const.size, ID, Const.islandGradient);
            IslandSaveManager.SaveIsland(dataArray, fileName);
        }

    }

    public void StartRender()
    {
        TerrainData _terrainData = new TerrainData();
        float[,] dataArray = IslandSaveManager.LoadIsland(fileName, ID);
        _terrainData.size = new Vector3(Const.size, Const.islandHeight, Const.size);
        _terrainData.heightmapResolution = Const.size - 1;      //Set how tall the resolution for the height of the terrain is
        _terrainData.SetHeights(0, 0, dataArray);               //Set the heights for the terrain
        terrain = Terrain.CreateTerrainGameObject(_terrainData);
        terrain.GetComponent<Terrain>().materialTemplate = _material;   //Set the material on the terrain
        terrain.transform.position = new Vector3(xCord * Const.islandDistance + xOffSet, -0.3f, zCord * Const.islandDistance + xOffSet);
        terrain.AddComponent<IslandOwner>().setStats(ID, "");
    }

    public void EndRender()
    {
        Destroy(terrain);
    }

}
