using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

// save system
public class SaveSystem : MonoBehaviour
{
    string path;
    BinaryFormatter formatter = new BinaryFormatter();
    
    private void Awake()
    {
        path = Application.dataPath + "/" + "Save" + SceneManager.GetActiveScene().name + ".binary"; // Application.persistantDataPath
        if (SceneManager.GetActiveScene().name == "SondreScene" || SceneManager.GetActiveScene().name == "PrivateIsland")
            Load();
    }

    private void OnApplicationQuit()
    {
        if (SceneManager.GetActiveScene().name == "SondreScene" || SceneManager.GetActiveScene().name == "PrivateIsland")
            Save();
    }

    // Loads save from file
    void Load()
    {
        // Load
        if (File.Exists(path))
        {
            // Opens stream
            FileStream stream = new FileStream(path, FileMode.Open);
            
            int numResources = (int)formatter.Deserialize(stream);
            for (int i = 0; i < numResources; i++)
            {
                ResourceSave resourceData = formatter.Deserialize(stream) as ResourceSave;

                Debug.Log("Prefabs/WorldResources/Raw resources/" + RemoveCopyInName(resourceData.resourceName));

                // Instantiate resource
                GameObject resourceObject = Resources.Load("Prefabs/WorldResources/Raw resources/" + RemoveCopyInName(resourceData.resourceName)) as GameObject;
                GameObject worldObject = LoadObject(resourceObject, resourceData.position, resourceData.rotation);
            }



            // Close stream
            stream.Close();
        }
        else
        {
            Debug.Log("No save file found");
        }
    }

    // Saves save to file
    void Save()
    {
        GameManager.isPaused = true;                                                        // Pauses game to ensure values does not update needlessly
        // Creates new file or overwrites the old one
        FileStream streamEnsureExists = new FileStream(path, FileMode.Create);
        streamEnsureExists.Close();

        // Opens stream to append to it
        FileStream stream = new FileStream(path, FileMode.Append);
        
        // Resources
        ResourceWorldObject[] worldResources = FindObjectsOfType<ResourceWorldObject>();    // Gets all resource objects
        formatter.Serialize(stream, worldResources.Length);                                 // Stores number of resources as a float

        for (int i = 0; i < worldResources.Length; i++)
        {
            float amount = worldResources[i].resourceAmount;
            string name = worldResources[i].ReturnType().transform.name;

            ResourceSave resourceData = new ResourceSave(amount, name, worldResources[i].transform.position, worldResources[i].transform.eulerAngles);
            formatter.Serialize(stream, resourceData);
        }


        // closes stream
        stream.Close();
    }

    // Removes "copy" from the end of the name
    string RemoveCopyInName(string name)
    {
        if (name.EndsWith("(Clone)"))
            return name.Substring(0, name.Length - 7);
        else
            return name;
    }

    // send spawned object, position and rotation and get spawned the world object in return
    GameObject LoadObject(GameObject spawnedObject, float[] pos, float[] rot)
    {
        return Instantiate(spawnedObject, FloatsToVectors(pos), Quaternion.Euler(FloatsToVectors(rot)));
    }

    // Makes array of three floats into Vector3
    Vector3 FloatsToVectors(float[] floats)
    {
        return new Vector3(floats[0], floats[1], floats[2]);
    }
}
