using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyPatrol : MonoBehaviour
{

    [SerializeField] Transform path;

    [SerializeField] bool inverse;

    List<Transform> waypoints = new List<Transform>();

    int index;

    NavMeshAgent agent;
    NavMeshPath navPath;


    float closest = float.MaxValue;
    Transform closestPoint;

    // Start is called before the first frame update
    void Start()
    {


        agent = GetComponent<NavMeshAgent>();

        navPath = new NavMeshPath();

        int childIndex = 0;

        foreach (Transform child in path)
        {
            waypoints.Add(child);

            NavMesh.CalculatePath(transform.position, child.position, NavMesh.AllAreas, navPath);


            float dist = 0f;

            for (int i = 1; i < navPath.corners.Length; i++)
            {
                dist += Vector3.Distance(navPath.corners[i - 1], navPath.corners[i]);
            }

            if (dist < closest)
            {
                index = childIndex;
                closest = dist;
                closestPoint = child;

            }

            childIndex++;
        }




        agent.SetDestination(closestPoint.position);



    }

    // Update is called once per frame
    void Update()
    {

        if (agent.remainingDistance < .1f)
        {
            // CLOCKWISE
            if (!inverse)
            {
                index++;

                if (!inverse && index > waypoints.Count - 1)
                    index = 0;
            }

            // COUNTER CLOCKWISE
            if (inverse)
            {
                index--;
                if (index < 0)
                    index = waypoints.Count - 1;
            }



            agent.SetDestination(waypoints[index].position);
        }
    }
}
