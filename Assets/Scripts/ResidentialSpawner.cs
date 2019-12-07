using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResidentialSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    [Range(2, 10)]
    public int _capacity;
    public List<GameObject> _citizenLiving;
    ObjectPool _pool;
    public string key;
    void Start()
    {
        _pool = GameObject.FindGameObjectWithTag("Manager").GetComponent<ObjectPool>();
        for (int i = 0; i < _capacity; i++)
        {
            GameObject temp = _pool.GetPooledObject(key);
            temp.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);
            temp.GetComponent<CitizenDestinationManager>().SetHome(this.GetComponent<GraphNode>());
            temp.transform.position = new Vector3(transform.position.x, transform.position.y + 2.0f, transform.position.z);
            temp.SetActive(true);
            _citizenLiving.Add(temp);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
