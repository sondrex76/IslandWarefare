using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

// save system
public class SaveSystem : MonoBehaviour
{
    string path;
    BinaryFormatter formatter = new BinaryFormatter();

    private void Awake()
    {
        path = Application.dataPath + "/save.binary"; // Application.persistantDataPath
        Load();
    }

    private void OnApplicationQuit()
    {
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
                Debug.Log(resourceData.resourceAmount + ", " + resourceData.resourceName);
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
            Debug.Log("DEBUG");
        }




        // closes stream
        stream.Close();
    }
}
