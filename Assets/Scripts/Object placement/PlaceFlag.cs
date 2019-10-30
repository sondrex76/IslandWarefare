// MoveToClickPoint.cs
    using UnityEngine;
    using UnityEngine.AI;
    using System.Collections.Generic;
    
    public class PlaceFlag : MonoBehaviour {
        bool _list;
        EventManager _eventManager;
        void Start() {
            _list = false;
            _eventManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<EventManager>();
        }
        
        void Update() {
            if (Input.GetButtonDown("Fire2"))
            {
                // GetButton locking something? ketkey for now
                _list = Input.GetKey(KeyCode.LeftShift);
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    
                    //suppose i have two objects here named obj1 and obj2.. how do i select obj1 to be transformed 
                    if (hit.transform != null)
                    {
                      
                        //transform.Translate(Time.deltaTime, 0, 0, Space.Self);
                        _eventManager._listenToFlag.Invoke(hit.point, _list); 
                           
                    }
                } 
                
                        
            }
        }
    }