using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

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

    [SerializeField]
    protected Material _islandMaterial;

    private int _ID;
    private string _userName;
    private string _passWord;
    private string _islandFileName = "/island" + 1;     //Debug for now
    private float _attackPower;
    private float _defensePower;
    private GameObject _island;

    private TerrainData data;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        loadIsland();
        _island = Terrain.CreateTerrainGameObject(data);
        _island.transform.position = new Vector3(-Const.size / 2, -0.1f, -Const.size / 2);
        _island.GetComponent<Terrain>().materialTemplate = _islandMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void loadIsland()
    {
        if (File.Exists(Application.persistentDataPath + _islandFileName))
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + _islandFileName, FileMode.Open);
            IslandSave save = (IslandSave)bf.Deserialize(file);
            file.Close();

            data = new TerrainData();
            data.size = new Vector3(Const.size, 400, Const.size);
            data.heightmapResolution = Const.size - 1;
            data.SetHeights(0, 0, save.ListToArray(Const.size, Const.size));
            
        }
    }
}
