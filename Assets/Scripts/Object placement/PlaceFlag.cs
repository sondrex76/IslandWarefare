// MoveToClickPoint.cs
    using UnityEngine;
    using UnityEngine.AI;
    using System.Collections.Generic;
    
    public class PlaceFlag : MonoBehaviour {
        [SerializeField]
        float ok;
        EventManager _eventManager;
        void Start() {
            _eventManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<EventManager>();
        }
        
        void Update() {
            if (Input.GetButtonDown("Fire2"))
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    //suppose i have two objects here named obj1 and obj2.. how do i select obj1 to be transformed 
                    if (hit.transform != null)
                    {
                        
                        //transform.Translate(Time.deltaTime, 0, 0, Space.Self);
                        _eventManager._listenToFlag.Invoke(hit.point, true); 
                           
                    }
                }                
            }
        }
    }