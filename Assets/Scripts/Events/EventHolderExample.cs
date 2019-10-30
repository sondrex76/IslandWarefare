using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHolderExample : MonoBehaviour
{
    public void MessageToSubscribedElements(string str)
    {
        Debug.Log(str + " and my position is " + transform.position);
    }
}
