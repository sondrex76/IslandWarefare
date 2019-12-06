using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CitizenDestinationManager : MonoBehaviour
{
    [Range(0, 100)]
    public float _speed;
    bool _AgentHasReached = true;
    Vector3 _destination;
    Graph _graph;
    GraphNode _currentNode;
    Queue<GraphNode> _path;
    GraphNode _home;
    GraphNode _work;
    GraphNode _shop;
    private int _randomAStarUpdate;

    // Start is called before the first frame update
    void Start()
    {
        _graph = GameObject.FindGameObjectWithTag("Graph").GetComponent<Graph>();
        _destination = new Vector3();
        _destination = transform.position;
        _path = new Queue<GraphNode>();
        _currentNode = _home;
        List<GraphNode> work = new List<GraphNode>();
        if (_graph.Nodes.Any())
        {
            foreach (var x in _graph.Nodes.Where(i => i._attribute == GraphNode.Attribute.Office))
            {
                work.Add(x);
            }
            System.Random randomWork = new System.Random();
            _work = work[randomWork.Next(0, work.Count)];
            _work.GetComponent<OfficeManager>().AddWorker(gameObject);
        }
        SecureRandom rng = new SecureRandom();
        _randomAStarUpdate = rng.Next(1, 1000);
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _destination, Time.deltaTime * _speed);
        if (_path != null)
        {

            
            if (_path.Count == 0 && _graph.Nodes.Any() && Time.frameCount % _randomAStarUpdate == 0)
            {
                if (_currentNode == _home)
                {
                    _path = AStar.FindShortestPath(_home, _work, _graph);
                    _currentNode = _work;
                }
                else if (_currentNode == _work)
                {
                    List<GraphNode> shops = new List<GraphNode>();
                    foreach (var x in _graph.Nodes.Where(i => i._attribute == GraphNode.Attribute.Commnerical))
                    {
                        shops.Add(x);
                    }
                    System.Random randomShop = new System.Random();
                    _shop = shops[randomShop.Next(0, shops.Count)];
                    _path = AStar.FindShortestPath(_work, _shop, _graph);
                    _currentNode = _shop;
                }
                else if (_currentNode == _shop)
                {
                    _path = AStar.FindShortestPath(_shop, _home, _graph);
                    _currentNode = _home;
                }
            }
        }
        if (_AgentHasReached)
        {
            AStarWalk();
        }
        
        if (Vector3.SqrMagnitude(transform.position - _destination) <= 5.0f)
        {
            _AgentHasReached = true;
        }
        
    }

    public void SetHome(GraphNode home)
    {
        _home = home;
    }

    public void SetWork(GraphNode work)
    {
        _work = work;
    }


    void AStarWalk() {
      if (_path != null) {
        if (_path.Count > 0)
            {
                _destination = _path.Dequeue().transform.position;
                _AgentHasReached = false;
            }
      }
    }
}


