using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise
{
    private float gradient = 25f;       //The roundness of the map, higher makes it rounder (flatter for plane)

    //Uses ID of Island to get seed
    public float[,] GetPerlinNoise(int xWidth, int zWidth, float xCoord, float zCoord)
    {
        float[,] map = new float[xWidth, zWidth];

        for (int x = 0; x < xWidth; x++)
        {
            for (int z = 0; z < zWidth; z++)
            {
                float X = (xCoord + x) / gradient;
                float Z = (zCoord + z) /  gradient;
                map[x,z] = Mathf.PerlinNoise(X , Z) - distanceSquared(x, z, xWidth, zWidth);
            }
        }
        
        return map;
    }

    private float distanceSquared(int x, int z, int xSize, int zSize)
    {
        float dx = 2 * (float)x / xSize - 1;
        float dz = 2 * (float)z / zSize - 1;

        return dx * dx + dz * dz;
    }
}
