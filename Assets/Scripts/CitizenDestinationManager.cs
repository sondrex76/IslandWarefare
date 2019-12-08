using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

public class CitizenDestinationManager : MonoBehaviour
{
    [Range(0, 100)]
    public float _speed;
    bool _AgentHasReached = true;
    Animator _animator;
    MeshRenderer _meshRenderer;
    Vector3 _destination;
    Graph _graph;
    GraphNode _currentNode;
    Queue<GraphNode> _path;
    public List<GraphNode> _viewPath; 
    GraphNode _home;
    GraphNode _work;
    GraphNode _shop;
    private float _randomAStarUpdate;
    
    private Rigidbody _rigidbody;
    float timeOfDay; // Should be a static global for everything

    [System.Serializable]
    struct InternalCitizenState
    {
        public int money;
        public float wakingUp;
        public float workTime;
        public float lunchTime;
        public float workDoneTime;
        public float dinnerTime;
        public float bedTime;
        public float awakeDuration;
        public float awakeLimit;
    }
    
    [SerializeField] InternalCitizenState citizenStateInfo;


    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _graph = GameObject.FindGameObjectWithTag("Graph").GetComponent<Graph>();

        citizenStateInfo = new InternalCitizenState();
        
        _destination = new Vector3();
        _destination = transform.position;
        _currentNode = _home;

        _path = new Queue<GraphNode>();
        _viewPath = new List<GraphNode>();


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
        _randomAStarUpdate = rng.NextFloat(1, 20);

    }

    private void FixedUpdate()
    {
        
        

    }

    // Update is called once per frame
    void Update()
    {
        
        if (_AgentHasReached)
        {
            AStarWalk();
            
        }
        OnFinishedPath();
        if (Vector3.SqrMagnitude(transform.position - _destination) <= 5.0f)
        {
            _AgentHasReached = true;
        }
        /// This conversion is more or less just for debugging to view Queue in editor
        /// Not should not be used as its expensive and complains a lot
        ///_viewPath = _path.ToList();
        ///
        if (!_rigidbody.IsSleeping())
        {
            transform.position = Vector3.MoveTowards(transform.position, _destination, Time.deltaTime * _speed);
            //rigidbody.velocity += new Vector3(0f, Physics.gravity.y * Time.deltaTime, 0f);
        }

        if (_path.Count == 0 && _AgentHasReached)
        {
            _rigidbody.Sleep();
            _meshRenderer.enabled = false;
        }
        else
        {
            _rigidbody.WakeUp();
            _meshRenderer.enabled = true;
        }

    }

    void OnFinishedPath()
    {
        if (_path == null)
        {
            //_rigidbody.Sleep();
            //if (Time.time % _randomAStarUpdate == 0)
            //{
            //    switch (Random.Range(0, 2))
            //    {
            //        case 0: _animator.SetBool("shouldHome", true); break;
            //        case 1: _animator.SetBool("shouldWork", true); break;
            //        case 2: _animator.SetBool("shouldShop", true); break;
            //    }
            //}
            
            return;
        }
        if (_path.Count <= 0)
        {
            //_rigidbody.Sleep();
            switch (_currentNode._attribute)
            {
                case GraphNode.Attribute.Commnerical:
                    _animator.SetBool("shouldShop", true);
                    // Go home or back to work
                    return;
                case GraphNode.Attribute.Industrial:
                    // Not Implemented
                    return;
                case GraphNode.Attribute.Millitary:
                    // Not implemented
                    return;
                case GraphNode.Attribute.Office:
                    _animator.SetBool("shouldWork", true);
                    // Figure out wheter agent should go home or shop for lunch
                    return;
                case GraphNode.Attribute.Residential:
                    // Figure out wheter agent should go to work, go shop or go on a walk
                    _animator.SetBool("shouldHome", true);
                    return;
                default:
                    _animator.SetBool("shouldHome", true);
                    // Find out where agent should go
                    return;
            }
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

    // Finds closest node for on the fly A* purposes
    GraphNode findClosestNode(Graph graph)
    {
        GraphNode closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach(GraphNode node in graph.Nodes)
        {
            Vector3 differnce = node.transform.position - position;
            float currentDistance = differnce.sqrMagnitude;
            if(currentDistance <= distance)
            {
                closest = node;
                distance = currentDistance;
            }
        }
        return closest;
    }

    public void CitizenStateManager(Animator animator, AnimatorStateInfo stateInfo, string state)
    {
        StartCoroutine(Example(state));
        
        
        
    }

    IEnumerator Example(string state)
    {
        yield return new WaitForSecondsRealtime(_randomAStarUpdate);
        
        switch (state)
        {
            case "Work":
                {
                    List<GraphNode> shops = new List<GraphNode>();
                    foreach (var x in _graph.Nodes.Where(i => i._attribute == GraphNode.Attribute.Commnerical))
                    {
                        shops.Add(x);
                    }
                    if (shops.Count > 0)
                    {
                        System.Random randomShop = new System.Random();
                        _shop = shops[randomShop.Next(0, shops.Count)];
                        _path = AStar.FindShortestPath(findClosestNode(_graph), _shop, _graph);
                        _currentNode = _shop;
                        _animator.SetBool("shouldWork", false);
                        
                    }
                    
                }
                break;
            case "Shop":
                {
                    _path = AStar.FindShortestPath(findClosestNode(_graph), _home, _graph);
                    _currentNode = _home;
                    _animator.SetBool("shouldShop", false);
                }
                break;
            case "Random":
                break;
            case "Idle":
                break;
            case "Home":
                {
                    _path = AStar.FindShortestPath(findClosestNode(_graph), _work, _graph);
                    _currentNode = _work;
                    _animator.SetBool("shouldHome", false);
                }
                break;
            default:
                {
                    _path = AStar.FindShortestPath(findClosestNode(_graph), _home, _graph);
                    _currentNode = _home;
                    //_animator.SetBool("shouldHome", false);
                    break;
                }
                
        }
    }

}


