using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

public class CitizenDestinationManager : MonoBehaviour
{
    [Range(0, 100)]
    public float speed;
    
    Animator animator;
    MeshRenderer meshRenderer;

    Vector3 destination;
    Graph graph;
    GraphNode currentNode;
    Queue<GraphNode> path;
    GraphNode home;
    GraphNode work;
    GraphNode shop;
    private float randomAStarUpdate;
    bool AgentHasReached = true;

    private Rigidbody rigidbody;



    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        meshRenderer = GetComponent<MeshRenderer>();
        graph = GameObject.FindGameObjectWithTag("Graph").GetComponent<Graph>();

        
        destination = new Vector3();
        destination = transform.position;
        currentNode = home;

        path = new Queue<GraphNode>();


        List<GraphNode> work = new List<GraphNode>();
        if (graph.Nodes.Any())
        {
            foreach (var node in graph.Nodes.Where(node => node.attribute == GraphNode.Attribute.Office))
            {
                work.Add(node);
            }
            if (work.Count > 0)
            {
                System.Random randomWork = new System.Random();
                this.work = work[randomWork.Next(0, work.Count)];
                this.work.GetComponent<OfficeNode>().AddCitizen(gameObject);
            } else
            {
                destination = new Vector3();
                destination = transform.position;
                currentNode = home;

                ObjectPool pool = GameObject.FindGameObjectWithTag("Manager").GetComponent<ObjectPool>();

                pool.AddObjcetToPool("Simple Citizen", gameObject);
                gameObject.SetActive(false);
            }
            
        }

        SecureRandom rng = new SecureRandom();
        randomAStarUpdate = rng.NextFloat(1, 20);

    }

    private void FixedUpdate()
    {
        Vector3 local = transform.position;
        Vector3 target = destination;
        local.y = 0;
        target.y = 0;
        if (Vector3.SqrMagnitude(local - target) <= 1f)
        {
            AgentHasReached = true;
        }
        if (animator.GetBool("IsWalking"))
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);
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
        Debug.Log("An error has occured agent will be reset");
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        meshRenderer = GetComponent<MeshRenderer>();
        graph = GameObject.FindGameObjectWithTag("Graph").GetComponent<Graph>();


        destination = new Vector3();
        destination = transform.position;
        currentNode = home;

        path = new Queue<GraphNode>();


        List<GraphNode> work = new List<GraphNode>();
        if (graph.Nodes.Any())
        {
            foreach (var node in graph.Nodes.Where(node => node.attribute == GraphNode.Attribute.Office))
            {
                work.Add(node);
            }
            System.Random randomWork = new System.Random();
            this.work = work[randomWork.Next(0, work.Count)];
            this.work.GetComponent<OfficeNode>().AddCitizen(gameObject);
        }

        SecureRandom rng = new SecureRandom();
        randomAStarUpdate = rng.NextFloat(1, 20);
    }

    void OnFinishedPath()
    {
        if (path == null)
        {
            if (Time.deltaTime % randomAStarUpdate == 0)
            {
                ResetOnError();
            }
        }

        if (path.Count <= 0 && AgentHasReached)
        {
            animator.SetBool("IsWalking", false);
            switch (currentNode.attribute)
            {
                case GraphNode.Attribute.Commnerical:
                    //animator.SetBool("shouldShop", true);
                    // Go home or back to work
                    return;
                case GraphNode.Attribute.Industrial:
                    // Not Implemented
                    return;
                case GraphNode.Attribute.Millitary:
                    // Not implemented
                    return;
                case GraphNode.Attribute.Office:
                    animator.SetBool("IsWorking", true);
                    // Figure out wheter agent should go home or shop for lunch
                    return;
                case GraphNode.Attribute.Residential:
                    // Figure out wheter agent should go to work, go shop or go on a walk
                    animator.SetBool("IsHome", true);
                    return;
                default:
                    animator.SetBool("IsHome", true);
                    // Find out where agent should go
                    return;
            }
        }
        

    }


    public void SetHome(GraphNode home)
    {
        this.home = home;
    }

    public void SetWork(GraphNode work)
    {
        this.work = work;
    }

    public void SleepAgent()
    {
        rigidbody.Sleep();
        meshRenderer.enabled = false;
    }
    public void WakeAgent()
    {
        rigidbody.WakeUp();
        meshRenderer.enabled = true;
    }


    public void AStarWalk() {
      if (path != null && AgentHasReached) {
        if (path.Count > 0)
            {
                destination = path.Dequeue().transform.position;
                AgentHasReached = false;
            }
      }
    }

    // Finds closest node for on the fly A* purposes
    GraphNode FindClosestNode(Graph graph)
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
        yield return new WaitForSecondsRealtime(randomAStarUpdate);
        
        switch (state)
        {
            case "Work":
                {
                    path = AStar.FindShortestPath(work, home, graph);
                    currentNode = home;
                    animator.SetBool("IsWorking", false);
                    animator.SetBool("IsWalking", true);
                }
                break;
            case "Home":
                {
                    path = AStar.FindShortestPath(home, work, graph);
                    currentNode = work;
                    animator.SetBool("IsHome", false);

                    animator.SetBool("IsWalking", true);
                }
                break;
            default:
                {
                    path = AStar.FindShortestPath(FindClosestNode(graph), home, graph);
                    currentNode = home;
                    //animator.SetBool("shouldHome", false);
                    break;
                }
                
        }
    }

}


