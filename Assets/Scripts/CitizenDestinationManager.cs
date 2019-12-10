using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

public class CitizenDestinationManager : MonoBehaviour
{
    [Range(0, 100)]
    public float _speed;
    
    Animator _animator;
    MeshRenderer _meshRenderer;

    Vector3 _destination;
    Graph _graph;
    GraphNode _currentNode;
    Queue<GraphNode> _path;
    GraphNode _home;
    GraphNode _work;
    GraphNode _shop;
    private float _randomAStarUpdate;
    bool _AgentHasReached = true;

    private Rigidbody _rigidbody;



    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _graph = GameObject.FindGameObjectWithTag("Graph").GetComponent<Graph>();

        
        _destination = new Vector3();
        _destination = transform.position;
        _currentNode = _home;

        _path = new Queue<GraphNode>();


        List<GraphNode> work = new List<GraphNode>();
        if (_graph.Nodes.Any())
        {
            foreach (var node in _graph.Nodes.Where(node => node._attribute == GraphNode.Attribute.Office))
            {
                work.Add(node);
            }
            System.Random randomWork = new System.Random();
            _work = work[randomWork.Next(0, work.Count)];
            _work.GetComponent<OfficeNode>().AddCitizen(gameObject);
        }
        SecureRandom rng = new SecureRandom();
        _randomAStarUpdate = rng.NextFloat(1, 20);

    }

    private void FixedUpdate()
    {
        Vector3 local = transform.position;
        Vector3 target = _destination;
        local.y = 0;
        target.y = 0;
        if (Vector3.SqrMagnitude(local - target) <= 1f)
        {
            _AgentHasReached = true;
        }
        if (_animator.GetBool("IsWalking"))
        {
            transform.position = Vector3.MoveTowards(transform.position, _destination, Time.deltaTime * _speed);
            //rigidbody.velocity += new Vector3(0f, Physics.gravity.y * Time.deltaTime, 0f);
        }

    }

    // Update is called once per frame
    void Update()
    {
        OnFinishedPath();
    }

    // Incase agent gets stuck or something simillar 
    void ResetOnError()
    {
        Start();
        StartCoroutine(Example("Default"));
    }

    void OnFinishedPath()
    {
        if (_path == null) ResetOnError();

        if (_path.Count <= 0 && _AgentHasReached)
        {
            _animator.SetBool("IsWalking", false);
            switch (_currentNode._attribute)
            {
                case GraphNode.Attribute.Commnerical:
                    //_animator.SetBool("shouldShop", true);
                    // Go home or back to work
                    return;
                case GraphNode.Attribute.Industrial:
                    // Not Implemented
                    return;
                case GraphNode.Attribute.Millitary:
                    // Not implemented
                    return;
                case GraphNode.Attribute.Office:
                    _animator.SetBool("IsWorking", true);
                    // Figure out wheter agent should go home or shop for lunch
                    return;
                case GraphNode.Attribute.Residential:
                    // Figure out wheter agent should go to work, go shop or go on a walk
                    _animator.SetBool("IsHome", true);
                    return;
                default:
                    _animator.SetBool("IsHome", true);
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

    public void SleepAgent()
    {
        _rigidbody.Sleep();
        _meshRenderer.enabled = false;
    }
    public void WakeAgent()
    {
        _rigidbody.WakeUp();
        _meshRenderer.enabled = true;
    }


    public void AStarWalk() {
      if (_path != null && _AgentHasReached) {
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
        return graph.Nodes
            .OrderBy(node => (node.transform.position - transform.position).sqrMagnitude)
            .FirstOrDefault();
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
                    _path = AStar.FindShortestPath(_work, _home, _graph);
                    _currentNode = _home;
                    _animator.SetBool("IsWorking", false);
                    _animator.SetBool("IsWalking", true);
                }
                break;
            case "Home":
                {
                    _path = AStar.FindShortestPath(_home, _work, _graph);
                    _currentNode = _work;
                    _animator.SetBool("IsHome", false);

                    _animator.SetBool("IsWalking", true);
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


