using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandMapManager : MonoBehaviour
{
    [SerializeField]
    Camera camera;
    [SerializeField]
    AttackIsland attackIsland;
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.name == "Terrain")
                {
                    attackIsland.AttackPlayer(hit.collider.GetComponent<IslandOwner>()._islandID);
                }
            }
        }
    }
}
