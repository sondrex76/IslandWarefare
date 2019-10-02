using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        //Keeps info on whether there is an island next to it or not
        public int northIsland;
        public int eastIsland;
        public int southIsland;
        public int westIsland;

        public List<IslandTile> tiles;

       public Island(int x, int y, int islandID)
        {
            tiles = new List<IslandTile>();

            ID = islandID;
            xCord = x;
            yCord = y;
            IslandsNextTo = 0;

            //Always starts with no island next to it
            northIsland = -1;
            eastIsland = -1;
            southIsland = -1;
            westIsland = -1;

            //algorithm for placing the tiles

            int tilesPlaced = 1;

            int xOffSet = Random.Range(-20, 20);
            int yOffSet = Random.Range(-20, 20);

            IslandTile tile = new IslandTile(x * 75 + xOffSet, y * 75 + xOffSet);

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
        if (Input.GetKey(KeyCode.N))    
        {
            

            //First island is to be added in the middle of the map
            if (iles.Count == 0)
            {
                Island isle = new Island(0, 0, nextID++);
                nearbyIsle.Add(isle.ID);
                iles.Add(isle);
                isle = null;
                amount++;
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

                    Debug.Log($"{nextTo}, {nearbyIsle.Count}, {iles.Count}");

                    //Place the tile next to the chosen tile that already exist to ensure that the island is connected
                    switch (side)
                    {
                        //North
                        case 0:
                            if (iles[nearbyIsle[nextTo]].northIsland == -1)
                            {
                                x = iles[nearbyIsle[nextTo]].xCord;
                                y = iles[nearbyIsle[nextTo]].yCord + 1;
                            }
                            else placed = false;
                            break;

                        //West
                        case 1:
                            if (iles[nearbyIsle[nextTo]].westIsland == -1)
                            {
                                x = iles[nearbyIsle[nextTo]].xCord - 1;
                                y = iles[nearbyIsle[nextTo]].yCord;
                            }
                            else placed = false;
                            break;

                        //South
                        case 2:
                            if (iles[nearbyIsle[nextTo]].southIsland == -1)
                            {
                                x = iles[nearbyIsle[nextTo]].xCord;
                                y = iles[nearbyIsle[nextTo]].yCord - 1;
                            }
                            else placed = false;
                            break;

                        //East
                        case 3:
                            if (iles[nearbyIsle[nextTo]].eastIsland == -1)
                            {
                                x = iles[nearbyIsle[nextTo]].xCord + 1;
                                y = iles[nearbyIsle[nextTo]].yCord;
                            }
                            else placed = false;
                            break;

                        default:

                            placed = false;
                            break;
                    }

                } while (!placed);

                //Once a place is found, add the island
                Island isle = new Island(x, y, nextID++);

                //Look for all the islands next to it
                for (int i = nearbyIsle.Count - 1; i >= 0; i--)
                {
                    //North
                    if (isle.xCord == iles[nearbyIsle[i]].xCord && isle.yCord + 1 == iles[nearbyIsle[i]].yCord)
                    {
                        isle.IslandsNextTo++;
                        isle.northIsland = nearbyIsle[i];
                        iles[nearbyIsle[i]].IslandsNextTo++;
                        iles[nearbyIsle[i]].southIsland = isle.ID;

                        if (isle.IslandsNextTo == 4) break;
                        if (iles[nearbyIsle[i]].IslandsNextTo == 4)
                        {
                            nearbyIsle.RemoveAt(i);
                        }
                    }

                    //West
                    else if (isle.xCord - 1 == iles[nearbyIsle[i]].xCord && isle.yCord == iles[nearbyIsle[i]].yCord)
                    {
                        isle.IslandsNextTo++;
                        isle.westIsland = nearbyIsle[i];
                        iles[nearbyIsle[i]].IslandsNextTo++;
                        iles[nearbyIsle[i]].eastIsland = isle.ID;

                        if (isle.IslandsNextTo == 4) break;
                        if (iles[nearbyIsle[i]].IslandsNextTo == 4)
                        {
                            nearbyIsle.RemoveAt(i);
                        }
                    }

                    //South
                    else if (isle.xCord == iles[nearbyIsle[i]].xCord && isle.yCord - 1 == iles[nearbyIsle[i]].yCord)
                    {
                        isle.IslandsNextTo++;
                        isle.southIsland = nearbyIsle[i];
                        iles[nearbyIsle[i]].IslandsNextTo++;
                        iles[nearbyIsle[i]].northIsland = isle.ID;

                        if (isle.IslandsNextTo == 4) break;
                        if (iles[nearbyIsle[i]].IslandsNextTo == 4)
                        {
                            nearbyIsle.RemoveAt(i);
                        }
                    }


                    //East
                    else if (isle.xCord + 1 == iles[nearbyIsle[i]].xCord && isle.yCord == iles[nearbyIsle[i]].yCord)
                    {
                        isle.IslandsNextTo++;
                        isle.eastIsland = nearbyIsle[i];
                        iles[nearbyIsle[i]].IslandsNextTo++;
                        iles[nearbyIsle[i]].westIsland = isle.ID;

                        if (isle.IslandsNextTo == 4) break;
                        if (iles[nearbyIsle[i]].IslandsNextTo == 4)
                        {
                            nearbyIsle.RemoveAt(i);
                        }
                    }
                }
                
                if (isle.IslandsNextTo != 4) nearbyIsle.Add(isle.ID);

                iles.Add(isle);
                isle = null;
                amount++;
                Debug.Log($"current amount of islands: {amount}");
            }
        }
    }
}
