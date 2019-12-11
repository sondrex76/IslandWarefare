using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class UnityEventVector3 : UnityEvent<Vector3, bool> {}
public class EventManager : MonoBehaviour
{
  
    public UnityEventVector3 listenToFlag;
    
}
