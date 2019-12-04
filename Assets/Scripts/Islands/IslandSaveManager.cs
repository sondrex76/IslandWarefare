using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class IslandSaveManager
{
    public static void SaveIsland(float[,] dataArr, string fileName)
    {
        BinaryFormatter bf = new BinaryFormatter();

        Debug.Log(dataArr[Const.size / 2, Const.size / 2]);

        IslandSave save = new IslandSave(dataArr, Const.size, Const.size);
        FileStream file = File.Create(Application.persistentDataPath + fileName);
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("Should have been saved");
    }

    public static float[,] LoadIsland(string fileName, int ID)
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

        //Map was not saved, create a new one
        float[,] map = PerlinNoise.GetPerlinNoise(Const.size, Const.size, ID, Const.islandGradient);
        SaveIsland(map, fileName);

        return map;
    }

    //Only used in editormode for now, but there might be implementation to delete saves at some point.
    public static void DeleteMapSave(string fileName)
    {
        File.Delete(Application.persistentDataPath + fileName);
    }
}
