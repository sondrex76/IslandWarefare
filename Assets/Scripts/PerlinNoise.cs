﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise
{
    private float gradient = 20f;

    //Uses ID of Island to get seed
    public float[,] GetPerlinNoise(int xWidth, int zWidth, float xCoord, float zCoord, int seed)
    {
        float[,] map = new float[xWidth, zWidth];

        Random.InitState(seed);

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
