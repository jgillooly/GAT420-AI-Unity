using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using UnityEngine;

public class AiAutonomousAgent : AiAgent
{
    public AiPerception seekPerception = null;
    public AiPerception fleePerception = null;
    public AiPerception flockPerception = null;

    private void Update()
    {
        //seek
        if (seekPerception != null)
        {
            var gameObjects = seekPerception.GetGameObjects();
            if (gameObjects.Length > 0)
            {
                movement.ApplyForce(Seek(gameObjects[0]));
            }
        }

        //flee
        if (fleePerception != null)
        {
            var gameObjects = fleePerception.GetGameObjects();
            if (gameObjects.Length > 0)
            {
                movement.ApplyForce(Flee(gameObjects[0]));
            }
        }

        //flock
        if (flockPerception != null)
        {
            var gameObjects = flockPerception.GetGameObjects();
            if(gameObjects.Length > 0)
            {
                movement.ApplyForce(Cohesion(gameObjects));
                movement.ApplyForce(Separation(gameObjects, 3));
                movement.ApplyForce(Allignment(gameObjects));
            }
        }


        transform.position = Utilities.Wrap(transform.position, new Vector3(-10,-10,-10), new Vector3(10,10,10));
    }

    private Vector3 Seek(GameObject target)
    {
        Vector3 direction = target.transform.position - transform.position;
        return GetSteeringForce(direction);
    }

    private Vector3 Flee(GameObject target)
    {
        Vector3 direction = transform.position - target.transform.position;
        return GetSteeringForce(direction);
    }

    private Vector3 Cohesion(GameObject[] neigbors)
    {
        Vector3 positions = Vector3.zero;
        foreach (var neighbor in neigbors)
        {
            positions += neighbor.transform.position;
        }

        Vector3 center = positions / neigbors.Length;
        Vector3 direction = center - transform.position;
        Vector3 force = GetSteeringForce(direction);
        return force;
    }

    private Vector3 Separation(GameObject[] neighbors, float radius)
    {
        Vector3 separation = Vector3.zero;
        foreach (var neighbor in neighbors)
        {
            Vector3 direction = transform.position - neighbor.transform.position;
            if (direction.magnitude < radius)
            {
                separation += direction / direction.sqrMagnitude;
            }
        }

        Vector3 force = GetSteeringForce(separation);
        return force;
    }

    public Vector3 Allignment(GameObject[] neighbors) { 
        Vector3 velocities = Vector3.zero;
        foreach (var neighbor in neighbors)
        {
            velocities += neighbor.GetComponent<AiAgent>().movement.Velocity;
        }
        Vector3 average = velocities / neighbors.Length;
        Vector3 force = GetSteeringForce(average);
        return force;

    }



    private Vector3 GetSteeringForce(Vector3 direction)
    {
        Vector3 desired = direction.normalized * movement.maxSpeed;
        Vector3 steer = desired - movement.Velocity;
        Vector3 force = Vector3.ClampMagnitude(steer, movement.maxForce);

        return force;
    }
}
