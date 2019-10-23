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

    //Keeps info on whether there is an island next to it or not
    public int northIsland;
    public int eastIsland;
    public int southIsland;
    public int westIsland;

    protected Material _material;

    //Size is always 2^x + 1, and 33 might just be the best size
    protected int size = 129;
    private GameObject terrain;

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

        if (File.Exists(Application.persistentDataPath + "/island" + ID))
        {
            PerlinNoise noise = new PerlinNoise();

            TerrainData _terrainData = new TerrainData();
            //DiamondSquare();
            float[,] dataArray = noise.GetPerlinNoise(size, size, ID);
            _terrainData.size = new Vector3(size, 100, size);
            _terrainData.heightmapResolution = size - 1;
            //_terrainData.baseMapResolution = 64;
            //_terrainData.SetDetailResolution(64, 2);
            _terrainData.SetHeights(0, 0, dataArray);
        }

    }

    public void StartRender()
    {
        TerrainData _terrainData = new TerrainData();
        float[,] dataArray = LoadMap();
        _terrainData.SetHeights(0, 0, dataArray);
        terrain = Terrain.CreateTerrainGameObject(_terrainData);
        terrain.AddComponent<MeshRenderer>();
        terrain.GetComponent<MeshRenderer>().material = _material;
        terrain.transform.position = new Vector3(xCord * 350 + xOffSet, -0.1f, zCord * 350 + xOffSet);
    }

    public void EndRender()
    {
        Destroy(terrain);
    }
    
    private void SaveMap(float[,] dataArr)
    {
        BinaryFormatter bf = new BinaryFormatter();

        IslandSave save = new IslandSave(dataArr);
        FileStream file = File.Create(Application.persistentDataPath + "/island" + ID);
        bf.Serialize(file, save);
        file.Close();
    }

    private float[,] LoadMap()
    {
        if (File.Exists(Application.persistentDataPath + "/island" + ID))
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/island" + ID, FileMode.Open);
            IslandSave save = (IslandSave)bf.Deserialize(file);
            file.Close();

            return save._heightMap;
        }

        return null;
    }

    public void DeleteMapSave()
    {
        File.Delete(Application.persistentDataPath + "/island" + ID);
    }
}
