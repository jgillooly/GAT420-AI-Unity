using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class AiSphereCastPerception : AiPerception
{
    [SerializeField][Range(0.1f, 5)] float Radius = 0.5f;
    [SerializeField][Range(2, 50)] int numRaycast = 2;

    public void OnDrawGizmos()
    {
        Vector3[] directions = Utilities.GetDirectionsInCircle(numRaycast, maxAngle);

        foreach (Vector3 direction in directions)
        {
            Ray ray = new Ray(transform.position, transform.rotation * direction);
            Gizmos.color = Color.yellow;
            Ray debug = ray;
            debug.direction *= distance;
            Gizmos.DrawLine(gameObject.transform.position, gameObject.transform.position + (debug.direction * distance));
            Gizmos.DrawWireSphere(ray.origin + ray.direction * distance, 0.5f);
        }
    }

    public override GameObject[] GetGameObjects()
    {
        List<GameObject> result = new List<GameObject>();

        //get directions for raycasts
        Vector3[] directions = Utilities.GetDirectionsInCircle(numRaycast, maxAngle);

        foreach (Vector3 direction in directions)
        {
            Ray ray = new Ray(transform.position, transform.rotation * direction);
            if (Physics.SphereCast(ray, Radius, out RaycastHit hit, distance))
            {
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
                //don't percieve self
                if (hit.collider.gameObject == gameObject) continue;
                //if untagged or if selected tag
                if (tagName == "" || hit.collider.CompareTag(tagName))
                {
                        result.Add(hit.collider.gameObject);
                }
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * distance, Color.green);
            }


            
        }

        //return the list as an array
        return result.Distinct().ToList().ToArray();
    }

    public bool GetOpenDirection(ref Vector3 openDirection)
    {
        Vector3[] directions = Utilities.GetDirectionsInCircle(numRaycast, maxAngle);
        foreach (var direction in directions)
        {
            // cast ray from transform position towards direction (use game object orientation)
            Ray ray = new Ray(transform.position, transform.rotation * direction);
            // if there is NO raycast hit then that is an open direction
            if (!Physics.SphereCast(ray, Radius, out RaycastHit raycastHit, distance, layerMask))
            {
                Debug.DrawRay(ray.origin, ray.direction * distance, Color.green);
                // set open direction
                openDirection = ray.direction;
                return true;
            }
        }

        // no open direction
        return false;
    }

    public bool CheckDirection(Vector3 direction)
    {
        // create ray in direction (use game object orientation)
        Ray ray = new Ray(transform.position, transform.rotation * direction);
        // check ray cast
        return Physics.Raycast(ray, distance, layerMask);

    }
}
