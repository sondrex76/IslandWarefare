/*
 * Copyright (c) 2016, Ivo van der Marel
 * Released under MIT License (= free to be used for anything)
 * Enjoy :)
 */
 
using UnityEngine;
using UnityEngine.Events;
 
public class Selectable : MonoBehaviour
{
    internal bool isSelected
    {
        get
        {
            return _isSelected;
        }
        set
        {
            _isSelected = value;
            //Replace this with your custom code. What do you want to happen to a Selectable when it get's (de)selected?
            Renderer r = GetComponentInChildren<Renderer>();
            if (r != null)
            {
                r.material.color = value ? Color.red : Color.white;                
            }
        }
    }
 
    private bool _isSelected;
 
    void OnEnable()
    {
        RTSSelection.selectables.Add(this);
    }
 
    void OnDisable()
    {
        RTSSelection.selectables.Remove(this);
    }

 
}