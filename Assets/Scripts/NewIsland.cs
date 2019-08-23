using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NewIsland : MonoBehaviour
{

    List<Island> iles;

    class IslandTile
    {
        public int xPos;
        public int yPos;

        public IslandTile(int x, int y)
        {
            xPos = x;
            yPos = y;
        }
    }

    class Island
    {
        public int xCord;
        public int yCord;

        public List<IslandTile> tiles;

       public Island(int x, int y)
        {
            tiles = new List<IslandTile>();
            
            xCord = x;
            yCord = y;
            //algorithm for placing the tiles

            int tilesPlaced = 1;

            IslandTile tile = new IslandTile(x * 51, y * 51);

            tiles.Add(tile);

            tile = null;

            while (tilesPlaced < 75)
            {
                bool placed = true;

                int xPos, yPos;

                //get a random tile already for the island
                int nextTo = Random.Range(0, tiles.Count);
                int side = Random.Range(0, 4);

                //Place the tile next to the chosen tile that already exist to ensure that the island is connected
                switch(side)
                {
                    //North
                    case 0:
                        xPos = tiles[nextTo].xPos;
                        yPos = tiles[nextTo].yPos + 1;
                        break;

                    //West
                    case 1:
                        xPos = tiles[nextTo].xPos - 1;
                        yPos = tiles[nextTo].yPos;
                        break;

                    //South
                    case 2:
                        xPos = tiles[nextTo].xPos;
                        yPos = tiles[nextTo].yPos - 1;
                        break;

                    //East
                    case 3:
                        xPos = tiles[nextTo].xPos + 1;
                        yPos = tiles[nextTo].yPos;
                        break;

                    default:

                        xPos = tiles[nextTo].xPos;
                        yPos = tiles[nextTo].yPos;
                        break;
                }

                //Check if position is good

                for (int i = 0; i < tiles.Count; i++)
                {

                    //Make sure it does not overlap with any other tiles
                    if (xPos == tiles[i].xPos && yPos == tiles[i].yPos)
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
                    tile = new IslandTile(xPos, yPos);
                    tiles.Add(tile);
                    tile = null;

                }
            }

            for (int i = 0; i < tiles.Count; i++)
            {

                Vector3 vec3 = new Vector3(tiles[i].xPos, tiles[i].yPos, -1);
                Instantiate(GameObject.FindWithTag("Island"), vec3, Quaternion.identity);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        iles = new List<Island>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            
            Debug.Log("Test 1");

            //First island is to be added in the middle of the map
            if (iles.Count == 0)
            {
                Island isle = new Island(0, 0);
                iles.Add(isle);
                isle = null;
            }

            else
            {
                int near = Random.Range(0, iles.Count);
                int x = 0, y = 0;
                bool placed = false;

                Debug.Log("Test 2");

                //Find a place with no island on it, or next to it
                do
                {
                    placed = true;

                    Debug.Log("Test 3");

                    x = Random.Range(iles[near].xCord, iles[near].xCord + 4) - 2;
                    y = Random.Range(iles[near].yCord, iles[near].yCord + 4) - 2;

                    Debug.Log($"Test, x= {x} and y= {y}");

                    //Check if the new island collides too close with another island
                    for (int i = 0; i < iles.Count; i++)
                    {

                        //Center
                        if (x == iles[i].xCord && y == iles[i].yCord)
                        {
                            placed = false;
                            break;
                        }

                        //North
                        if (x == iles[i].xCord && y == iles[i].yCord + 1)
                        {
                            placed = false;
                            break;
                        }

                        //West
                        if (x == iles[i].xCord - 1 && y == iles[i].yCord)
                        {
                            placed = false;
                            break;
                        }

                        //South
                        if (x == iles[i].xCord && y == iles[i].yCord - 1)
                        {
                            placed = false;
                            break;
                        }

                        //East
                        if (x == iles[i].xCord + 1 && y == iles[i].yCord)
                        {
                            placed = false;
                            break;
                        }
                    }

                } while (!placed);

                //Once a place is found, add the island
                Island isle = new Island(x, y);
                iles.Add(isle);
                isle = null;
            }
        }
    }
}
