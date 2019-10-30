// MoveToClickPoint.cs
    using UnityEngine;
    using UnityEngine.AI;
    
    public class PlaceFlag : MonoBehaviour {
        NavMeshAgent agent;
        [SerializeField]
        float ok;
        Selectables _isSelected;

        void Start() {
            agent = GetComponent<NavMeshAgent>();
            Debug.Log(agent.gameObject.name);
            _isSelected = GetComponent<Selectables>();
        }
        
        void Update() {
            if(_isSelected.isSelected) 
            {
                if (Input.GetMouseButtonDown(1))
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        //suppose i have two objects here named obj1 and obj2.. how do i select obj1 to be transformed 
                        if (hit.transform != null)
                        {
                        
                            //transform.Translate(Time.deltaTime, 0, 0, Space.Self);
                            agent.SetDestination(hit.point);

                            
                        }
                    }
                }
            }
        }
    }
    /*
    Mouse click to [SEND] target location for [SELECTED] units
    Place [FLAG] on selected destination [FLAG's] only visible if having selected a unit going there (AOM style)
    Move [SELECTED] [UNITS] to targeted locations with go here in order with say shit + click or just go here with click
     */