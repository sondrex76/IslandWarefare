using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IslandSave
{
    public List<float> heigthMapList;

    public IslandSave(float[,] heightMap, int xSize, int zSize)
    {
        heigthMapList = new List<float>();

        for (int x = 0; x < xSize; x++)
        {
            for (int z = 0; z < zSize; z++)
            {
                heigthMapList.Add(heightMap[x, z]);
            }
        }
    }

    public float[,] ListToArray(int xSize, int zSize)
    {
        float[,] map = new float[xSize, zSize];

        for (int x = 0; x < xSize; x++)
        {
            for (int z = 0; z < zSize; z++)
            {
                map[x, z] = heigthMapList[x * xSize + z];
            }
        }

        return map;
    }
}
