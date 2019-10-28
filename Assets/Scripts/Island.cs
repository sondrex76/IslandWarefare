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

    private string fileName;

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
            Debug.Log(ID);
            PerlinNoise noise = new PerlinNoise();

            TerrainData _terrainData = new TerrainData();
            //DiamondSquare();
            float[,] dataArray = noise.GetPerlinNoise(Const.size, Const.size, xCord * Const.islandDistance + xOffSet, zCord * Const.islandDistance + xOffSet, ID);
            SaveMap(dataArray);
            
        }

    }

    public void StartRender()
    {
        TerrainData _terrainData = new TerrainData();
        float[,] dataArray = LoadMap();
        _terrainData.size = new Vector3(Const.size, 100, Const.size);
        _terrainData.heightmapResolution = Const.size - 1;
        _terrainData.SetHeights(0, 0, dataArray);
        terrain = Terrain.CreateTerrainGameObject(_terrainData);
        terrain.GetComponent<Terrain>().materialTemplate = _material;
        terrain.transform.position = new Vector3(xCord * Const.islandDistance + xOffSet, -0.1f, zCord * Const.islandDistance + xOffSet);
    }

    public void EndRender()
    {
        Destroy(terrain);
    }
    
    private void SaveMap(float[,] dataArr)
    {
        BinaryFormatter bf = new BinaryFormatter();

        Debug.Log(dataArr[Const.size / 2, Const.size / 2]);

        IslandSave save = new IslandSave(dataArr, Const.size, Const.size);
        FileStream file = File.Create(Application.persistentDataPath + fileName);
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("Should have been saved");
    }

    private float[,] LoadMap()
    {
        if (File.Exists(Application.persistentDataPath + fileName))
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + fileName, FileMode.Open);
            IslandSave save = (IslandSave)bf.Deserialize(file);
            file.Close();

            return save.ListToArray(Const.size, Const.size);
        }
        else Debug.Log("File does not exist when trying to load it");

        //Map did not save, try deleting it
        PerlinNoise noise = new PerlinNoise();

        float[,]map = noise.GetPerlinNoise(Const.size, Const.size, xCord * Const.islandDistance + xOffSet, zCord * Const.islandDistance + xOffSet, ID);
        SaveMap(map);

        return map;
    }

    public void DeleteMapSave()
    {
        File.Delete(Application.persistentDataPath + fileName);
    }
}
