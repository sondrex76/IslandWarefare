using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    private static Player _instance;

    public static Player Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Player>();
            }

            return _instance;
        }
    }

    [System.Serializable]
    public class TerrainGenerated : UnityEvent
    {

    }

    [SerializeField]
    private TerrainGenerated generated;

    [SerializeField]
    protected Material _islandMaterial;

    private int ID;
    private string userName;               //Not used as of now
    private string islandFileName;
    private float attackPower;             //Not used as of now
    private float defensePower;            //Not used as of now
    private GameObject island;

    private TerrainData data;               //Could be made into an object that is temporary

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Loading Island");
        ID = PlayerPrefs.GetInt("ISLANDID");
        islandFileName = "/island" + ID;

        Debug.Log(Application.persistentDataPath);

        float[,] map = IslandSaveManager.LoadIsland(islandFileName, ID);
        data = new TerrainData {
            size = new Vector3(Const.size, Const.islandHeight, Const.size),
            heightmapResolution = Const.size - 1
            };
        data.SetHeights(0, 0, map);

        island = Terrain.CreateTerrainGameObject(data);
        island.transform.position = new Vector3((-Const.size * 8) / 2, 0, (-Const.size * 8) / 2);
        island.GetComponent<Terrain>().materialTemplate = _islandMaterial;

        generated.Invoke();
        Debug.Log("Loading Island Done");

    }

}
