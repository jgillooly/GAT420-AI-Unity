using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAgentSpawner : MonoBehaviour
{
    [SerializeField] AiAgent[] agents;
    [SerializeField] LayerMask layerMask;

    int index = 0;

    void Update()
    {
        //press tab to switch the agent to spawn
        if (Input.GetKeyDown(KeyCode.Tab)) index = (++index % agents.Length);

        //click to spawn or hold left ctrl and mouse button to spawn multiple
        if (Input.GetMouseButtonDown(0) || (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftControl)))
        {
            //get ray from camera to screen pos
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //raycast and see if it hits an object with layer
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100, layerMask))
            {
                //spawn agent at hit point and random rotation
                Instantiate(agents[index], hitInfo.point, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
            }
        }
    }
}
