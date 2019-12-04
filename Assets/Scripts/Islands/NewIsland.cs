using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewIsland : MonoBehaviour
{

    List<Island> iles;
    int nextID;
    List<int> nearbyIsle;
    List<int> renderedIslands;
    List<int> unrenderedIslands;

    [SerializeField]
    protected Material material;

    [SerializeField]
    private Camera _camera;
    private Vector3 _prevCamPos;
    protected int distance = 100000;   //Distance away from camera an island will be rendered

    //Debug
    protected int antIslands = 100;

    // Start is called before the first frame update
    void Start()
    {
        _prevCamPos = _camera.transform.position;
        iles = new List<Island>();
        nearbyIsle = new List<int>();
        renderedIslands = new List<int>();
        unrenderedIslands = new List<int>();
        nextID = 0;
        Random.InitState(91133);

        float temp = Time.time;
        GenerateIsland();
        Debug.Log("Time for MyExpensiveFunction: " + (Time.time - temp).ToString("f6"));
        
    }

    private void Update()
    {
        //Debug only
        if (Input.GetKeyDown(KeyCode.Q) && Application.platform== RuntimePlatform.WindowsEditor)
        {
            Debug.Log("Deleting island saves");
            foreach (Island ile in iles)
            {
                IslandSaveManager.DeleteMapSave(ile.fileName);
            }
        }

        //Only check if there needs to be an update to rendering if the camera has moved
        if (_prevCamPos != _camera.transform.position)
        {
            //Save new camera position
            _prevCamPos = _camera.transform.position;

            List<int> newRenders = new List<int>();
            List<int> deRenders = new List<int>();

            //Check every rendered island if they are outside render distance
            foreach (int id in renderedIslands)
            {
                if (!(iles[id].xCord * Const.islandDistance > _camera.transform.position.x - distance) || !(iles[id].xCord * Const.islandDistance < _camera.transform.position.x + distance) ||
                    !(iles[id].zCord * Const.islandDistance > _camera.transform.position.z - distance) || !(iles[id].zCord * Const.islandDistance < _camera.transform.position.z + distance))
                {
                    //Add the islands to a list for those that are being rendered
                    iles[id].EndRender();
                    deRenders.Add(id);
                }
            }

            //Check every unrendered island if they are insinde render distance
            foreach (int id in unrenderedIslands)
            {
                if (iles[id].xCord * Const.islandDistance > _camera.transform.position.x - distance && iles[id].xCord * Const.islandDistance < _camera.transform.position.x + distance &&
                    iles[id].zCord * Const.islandDistance > _camera.transform.position.z - distance && iles[id].zCord * Const.islandDistance < _camera.transform.position.z + distance)
                {
                    //Add the islands to a list for those that are being derendered
                    iles[id].StartRender();
                    newRenders.Add(id);
                }
            }
            
            foreach (int id in newRenders)
            {
                unrenderedIslands.Remove(id);
                renderedIslands.Add(id);
            }

            foreach (int id in deRenders)
            {
                renderedIslands.Remove(id);
                unrenderedIslands.Add(id);
            }
        }
    }

    // Update is called once per frame
    void GenerateIsland()
    {
        for (int j = 0; j < antIslands; j++)
        {


            //First island is to be added in the middle of the map
            if (iles.Count == 0)
            {
                Island isle = new Island(0, 0, nextID++, material);
                nearbyIsle.Add(isle.ID);
                iles.Add(isle);
                isle = null;
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
                                y = iles[nearbyIsle[nextTo]].zCord + 1;
                            }
                            else placed = false;
                            break;

                        //West
                        case 1:
                            if (iles[nearbyIsle[nextTo]].westIsland == -1)
                            {
                                x = iles[nearbyIsle[nextTo]].xCord - 1;
                                y = iles[nearbyIsle[nextTo]].zCord;
                            }
                            else placed = false;
                            break;

                        //South
                        case 2:
                            if (iles[nearbyIsle[nextTo]].southIsland == -1)
                            {
                                x = iles[nearbyIsle[nextTo]].xCord;
                                y = iles[nearbyIsle[nextTo]].zCord - 1;
                            }
                            else placed = false;
                            break;

                        //East
                        case 3:
                            if (iles[nearbyIsle[nextTo]].eastIsland == -1)
                            {
                                x = iles[nearbyIsle[nextTo]].xCord + 1;
                                y = iles[nearbyIsle[nextTo]].zCord;
                            }
                            else placed = false;
                            break;

                        default:

                            placed = false;
                            break;
                    }

                } while (!placed);

                //Once a place is found, add the island
                Island isle = new Island(x, y, nextID++, material);

                //Look for all the islands next to it
                for (int i = nearbyIsle.Count - 1; i >= 0; i--)
                {
                    //North
                    if (isle.xCord == iles[nearbyIsle[i]].xCord && isle.zCord + 1 == iles[nearbyIsle[i]].zCord)
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
                    else if (isle.xCord - 1 == iles[nearbyIsle[i]].xCord && isle.zCord == iles[nearbyIsle[i]].zCord)
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
                    else if (isle.xCord == iles[nearbyIsle[i]].xCord && isle.zCord - 1 == iles[nearbyIsle[i]].zCord)
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
                    else if (isle.xCord + 1 == iles[nearbyIsle[i]].xCord && isle.zCord == iles[nearbyIsle[i]].zCord)
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
            }
        }

        //Check for which islands are to start rendered
        for (int i = 0; i < iles.Count; i++)
        {
            if (iles[i].xCord * Const.islandDistance > _camera.transform.position.x - distance && iles[i].xCord * Const.islandDistance < _camera.transform.position.x + distance &&
                iles[i].zCord * Const.islandDistance > _camera.transform.position.z - distance && iles[i].zCord * Const.islandDistance < _camera.transform.position.z + distance)
            {
                renderedIslands.Add(iles[i].ID);
                iles[i].StartRender();
            }
            else unrenderedIslands.Add(iles[i].ID); 
        }
    }
}
