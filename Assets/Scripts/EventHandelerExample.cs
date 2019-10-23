using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandelerExample : MonoBehaviour
{
    public delegate void DelegateEventExample(string str);
    public static event DelegateEventExample delegateEventExample;
    
    // Debugs the text from all objects subscribed on the list
    void MessageToSubscribedElements(string str)
    {
        Debug.Log(str);
    }
    
    // When an object enters the area
    private void OnTriggerEnter(Collider other)
    {
        // Subscribes element
        if (other.transform.GetComponent<EventHolderExample>() != null)
            delegateEventExample += other.transform.GetComponent<EventHolderExample>().MessageToSubscribedElements;

        // Sends message to all subscribed elements
        if (delegateEventExample != null)
            delegateEventExample("Element " + other.name + "Just got into range");
    }

    private void OnTriggerExit(Collider other)
    {
        // unsubscribes element
        if (other.transform.GetComponent<EventHolderExample>() != null)
            delegateEventExample -= 
                other.transform.GetComponent<EventHolderExample>().MessageToSubscribedElements;

        // Sends message to all subscribed elements
        if (delegateEventExample != null)
            delegateEventExample("Element " + other.name + "Just went out of range");
    }
}
