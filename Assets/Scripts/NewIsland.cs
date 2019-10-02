using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewIsland : MonoBehaviour
{

    List<Island> iles;
    int amount, nextID;
    List<int> nearbyIsle;

    //Debug
    protected int antIslands = 1000;

    // Start is called before the first frame update
    void Start()
    {
        iles = new List<Island>();
        nearbyIsle = new List<int>();
        amount = 0;
        //For now, till the game is online
        nextID = 0;
        Random.InitState(91133);

        var temp = Time.time;
        GenerateIsland();
        Debug.Log("Time for MyExpensiveFunction: " + (Time.time - temp).ToString("f6"));
    }

    // Update is called once per frame
    void GenerateIsland()
    {
        for (int j = 0; j < antIslands; j++) {
            

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
