using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NewIsland : MonoBehaviour
{

    List<Island> iles;
    int amount, nextID;
    List<int> nearbyIsle;

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
        public int ID;
        public int xCord;
        public int yCord;
        public int IslandsNextTo;

        public List<IslandTile> tiles;

       public Island(int x, int y, int islandID)
        {
            tiles = new List<IslandTile>();

            ID = islandID;
            xCord = x;
            yCord = y;
            IslandsNextTo = 0;

            //algorithm for placing the tiles

            int tilesPlaced = 1;

            int xOffSet = Random.Range(-20, 20);
            int yOffSet = Random.Range(-20, 20);

            IslandTile tile = new IslandTile(x * 55 + xOffSet, y * 55 + xOffSet);

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
        nearbyIsle = new List<int>();
        amount = 0;
        //For now, till the game is online
        nextID = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))    
        {
            

            //First island is to be added in the middle of the map
            if (iles.Count == 0)
            {
                Island isle = new Island(0, 0, nextID++);
                nearbyIsle.Add(isle.ID);
                iles.Add(isle);
                isle = null;
                amount++;
                Debug.Log($"current amount of islands: {amount}");
            }

            else
            {
                int x = 0, y = 0;
                bool placed = false;
                

                //Find a place with no island on it, or next to it
                do
                {
                    placed = true;


                    int nextTo = Random.Range(0, nearbyIsle.Count);
                    int side = Random.Range(0, 4);

                    //Place the tile next to the chosen tile that already exist to ensure that the island is connected
                    switch (side)
                    {
                        //North
                        case 0:
                            x = iles[nearbyIsle[nextTo]].xCord;
                            y = iles[nearbyIsle[nextTo]].yCord + 1;
                            break;

                        //West
                        case 1:
                            x = iles[nearbyIsle[nextTo]].xCord - 1;
                            y = iles[nearbyIsle[nextTo]].yCord;
                            break;

                        //South
                        case 2:
                            x = iles[nearbyIsle[nextTo]].xCord;
                            y = iles[nearbyIsle[nextTo]].yCord - 1;
                            break;

                        //East
                        case 3:
                            x = iles[nearbyIsle[nextTo]].xCord + 1;
                            y = iles[nearbyIsle[nextTo]].yCord;
                            break;

                        default:

                            x = iles[nearbyIsle[nextTo]].xCord;
                            y = iles[nearbyIsle[nextTo]].yCord;
                            break;
                    }

                    //Check if the new island collides too close with another island
                    for (int i = 0; i < iles.Count; i++)
                    {

                        //Center
                        if (x == iles[i].xCord && y == iles[i].yCord)
                        {
                            placed = false;
                            break;
                        }
                    }

                } while (!placed);

                //Once a place is found, add the island
                Island isle = new Island(x, y, nextID++);

                for (int i = nearbyIsle.Count - 1; i >= 0; i--)
                {
                    //North
                    if (isle.xCord == iles[nearbyIsle[i]].xCord && isle.yCord + 1 == iles[nearbyIsle[i]].yCord)
                    {
                        isle.IslandsNextTo++;
                        iles[nearbyIsle[i]].IslandsNextTo++;

                        if (isle.IslandsNextTo == 4) break;
                        if (iles[i].IslandsNextTo == 4)
                        {
                            Debug.Log($"Hello");
                            nearbyIsle.RemoveAt(i);
                        }
                    }

                    //West
                    else if (isle.xCord - 1 == iles[nearbyIsle[i]].xCord && isle.yCord == iles[nearbyIsle[i]].yCord)
                    {
                        isle.IslandsNextTo++;
                        iles[nearbyIsle[i]].IslandsNextTo++;

                        if (isle.IslandsNextTo == 4) break;
                        if (iles[i].IslandsNextTo == 4)
                        {
                            Debug.Log($"Hello");
                            nearbyIsle.RemoveAt(i);
                        }
                    }

                    //South
                    else if (isle.xCord == iles[nearbyIsle[i]].xCord && isle.yCord - 1 == iles[nearbyIsle[i]].yCord)
                    {
                        isle.IslandsNextTo++;
                        iles[nearbyIsle[i]].IslandsNextTo++;

                        if (isle.IslandsNextTo == 4) break;
                        if (iles[i].IslandsNextTo == 4)
                        {
                            Debug.Log($"Hello");
                            nearbyIsle.RemoveAt(i);
                        }
                    }


                    //East
                    else if (isle.xCord + 1 == iles[nearbyIsle[i]].xCord && isle.yCord == iles[nearbyIsle[i]].yCord)
                    {
                        isle.IslandsNextTo++;
                        iles[nearbyIsle[i]].IslandsNextTo++;

                        if (isle.IslandsNextTo == 4) break;
                        if (iles[i].IslandsNextTo == 4)
                        {
                            Debug.Log($"Hello");
                            nearbyIsle.RemoveAt(i);
                        }
                    }
                }

                
                if (isle.IslandsNextTo != 4) nearbyIsle.Add(isle.ID);

                iles.Add(isle);
                isle = null;
                amount++;
                Debug.Log($"current amount of islands: {amount}");
                Debug.Log($"current amount of islands not filled: {nearbyIsle.Count}");
            }
        }
    }
}
