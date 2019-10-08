using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour
{
    public int ID;
    public int xCord;
    public int zCord;
    public int IslandsNextTo;

    //Keeps info on whether there is an island next to it or not
    public int northIsland;
    public int eastIsland;
    public int southIsland;
    public int westIsland;

    public List<IslandTile> tiles;

    public Island(int x, int z, int islandID)
    {
        tiles = new List<IslandTile>();

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

        int tilesPlaced = 1;

        int xOffSet = Random.Range(-20, 20);
        int zOffSet = Random.Range(-20, 20);

        IslandTile tile = new IslandTile(x * 75 + xOffSet, z * 75 + xOffSet);

        tiles.Add(tile);

        tile = null;

        while (tilesPlaced < 75)
        {
            bool placed = true;

            int xPos, zPos;

            //get a random tile already for the island
            int nextTo = Random.Range(0, tiles.Count);
            int side = Random.Range(0, 4);

            //Place the tile next to the chosen tile that already exist to ensure that the island is connected
            switch (side)
            {
                //North
                case 0:
                    xPos = tiles[nextTo].xPos;
                    zPos = tiles[nextTo].zPos + 1;
                    break;

                //West
                case 1:
                    xPos = tiles[nextTo].xPos - 1;
                    zPos = tiles[nextTo].zPos;
                    break;

                //South
                case 2:
                    xPos = tiles[nextTo].xPos;
                    zPos = tiles[nextTo].zPos - 1;
                    break;

                //East
                case 3:
                    xPos = tiles[nextTo].xPos + 1;
                    zPos = tiles[nextTo].zPos;
                    break;

                default:

                    xPos = tiles[nextTo].xPos;
                    zPos = tiles[nextTo].zPos;
                    break;
            }

            //Check if position is good

            for (int i = 0; i < tiles.Count; i++)
            {

                //Make sure it does not overlap with any other tiles
                if (xPos == tiles[i].xPos && zPos == tiles[i].zPos)
                {
                    //if the tile overlaps, it won't be placed and there is no need to check the rest of the tiles
                    placed = false;
                    break;
                }
            }

            //If it can be placed, add it to the list
            if (placed)
            {
                tilesPlaced++;
                tile = new IslandTile(xPos, zPos);
                tiles.Add(tile);
                tile = null;

            }
        }

        for (int i = 0; i < tiles.Count; i++)
        {

            Vector3 vec3 = new Vector3((float)tiles[i].xPos / 10f, 1, (float)tiles[i].zPos / 10f);
            Instantiate(GameObject.FindWithTag("Island"), vec3, GameObject.FindWithTag("Island").transform.rotation);
        }
    }
}
