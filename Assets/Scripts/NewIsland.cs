using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewIsland : MonoBehaviour
{

    //For testing
    int a = 0, b = 0;

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

            while (tilesPlaced < 25)
            {
                bool placed = true;

                int xPos, yPos;

                //get a random tile already for the island
                int nextTo = Random.Range(0, tiles.Count - 1);
                int side = Random.Range(0, 3);

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

            for (int i = 0; i < 25; i++)
            {

                Vector3 vec3 = new Vector3(tiles[i].xPos, tiles[i].yPos, -1);
                Instantiate(GameObject.FindWithTag("Island"), vec3, Quaternion.identity);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Island isle = new Island(a, b);
            a++;
            if (a == 10) { b++; a = 0; }
        }
    }
}
