using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AiDistancePerception : AiPerception
{
    public override GameObject[] GetGameObjects()
    {
        List<GameObject> result = new List<GameObject>();

        Collider[] colliders = Physics.OverlapSphere(transform.position, distance);
        foreach (Collider collider in colliders)
        {
            //don't percieve self
            if (collider.gameObject == gameObject) continue;

            //if untagged or if selected tag
            if (tagName == "" || collider.CompareTag(tagName))
            {
                // calculate angle from transform forward vector to direction of game object
                Vector3 direction = (collider.transform.position - transform.position).normalized;
                float angle = Vector3.Angle(transform.forward, direction);
                // if angle is less than max angle, add game object
                if (angle <= maxAngle)
                {
                    result.Add(collider.gameObject);
                }
            }
        }

        //return the list as an array
        return result.ToArray();
    }
}
