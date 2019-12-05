using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllMapView : MonoBehaviour
{

    [SerializeField]
    float speed;
    InputManager inputManager;
    bool cameraInTransit = false;
    public Vector3 playerIsland;


    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameManager.inputManager;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!cameraInTransit)
        {
            if (Input.GetKey(inputManager.bindings[(int)InputManager.Actions.BACKWARDS]))
            {
                transform.Translate(transform.forward * speed);
            }


            if (Input.GetKey(inputManager.bindings[(int)InputManager.Actions.FORWARDS]))
            {
                transform.Translate(transform.forward * -speed);
            }


            if (Input.GetKey(inputManager.bindings[(int)InputManager.Actions.LEFT]))
            {
                transform.Translate(transform.right * -speed);
            }

            if (Input.GetKey(inputManager.bindings[(int)InputManager.Actions.RIGHT]))
            {
                transform.Translate(transform.right * speed);
            }
        }

    }



    public void CenterCamera()
    {
        if (!cameraInTransit)
        {
            cameraInTransit = true;
            StartCoroutine(LerpCamera());
        }
    }

    IEnumerator LerpCamera()
    {
        var duration = 1.0f;
        for (var t = 0.0f; t < duration; t += Time.deltaTime)
        {
            Vector3 isPos = new Vector3((Const.size * 8) / 2 + playerIsland.x, 200, (Const.size * 8) / 2 + playerIsland.z);
            transform.position = Vector3.Lerp(transform.position, isPos, t / duration);
            yield return 0;
        }

        cameraInTransit = false;

    }

}


