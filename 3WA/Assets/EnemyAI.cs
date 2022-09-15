using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Transform waypointParent;
    [SerializeField] bool inverse;
    [SerializeField] bool stopAtWaypoint;
    
    bool isWaiting;


    List<Transform> waypoints = new List<Transform>();

    Transform closestPoint;
    float closestDistance = -1f;

    NavMeshAgent agent;

    int index;



    // Start is called before the first frame update
    void Start()
    {
        
        // ON RECUPERE LE NAV MESH AGENT
        agent = GetComponent<NavMeshAgent>();


        // POUR CHAQUE ENFANT DU "PATH"
        foreach (Transform child in waypointParent)
        {
            // ON RECUPERE LES POINTS DE PASSAGE DANS UNE LISTE
            waypoints.Add(child);

            // ON CALCULE LA DISTANCE ENTRE LE PERSONNAGE ET LE POINT DE PASSAGE
            float dist = GetPathDistance(child);

            // SI CETTE DISTANCE EST INFERIEUR A LA DISTANCE LA PLUS PROCHE
            if(closestDistance < 0 || dist < closestDistance)
            {
                // ALORS CE POINT DE PASSAGE DEVIENT LA NOUVELLE DISTANCE LA PLUS PROCHE
                closestDistance = dist;
                closestPoint = child;
            }
            
        }

        // ON RECUPERE L'INDEX DU POINT DE PASSAGE LE PLUS PROCHE
        index = waypoints.IndexOf(closestPoint);

        // ON SET LA DESTINATION
        agent.SetDestination(waypoints[index].position);

    }

    // RETOURNE LA DISTANCE DU PATH ENTRE MON OBJET ET UNE TARGET
    float GetPathDistance(Transform target)
    {

        // ON INITIALISE UN NOUVEAU PATH
        NavMeshPath path = new NavMeshPath();

        // ON CALCULE LE PATH ENTRE DEUX POSITIONS
        NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);

        // EN CAS DE POINT INACCESSIBLE
        if(path.status == NavMeshPathStatus.PathInvalid)
        {
            Debug.LogError(target.name + " has invalid position", target);
            return float.MaxValue;
        }


        // ON CALCULE LA LONGUEUR DU PATH
        float pathDistance = 0;

        // POUR CHAQUE COIN DU PATH
        for (int i = 1; i < path.corners.Length; i++)
        {
            // ON ADDITIONNE LES LONGUEURS ENTRE LE COIN PRECEDANT ET LE COIN ACTUEL
            pathDistance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
        }



        // ON RETOURNE SA VALEUR
        return pathDistance;
    }


    // Update is called once per frame
    void Update()
    {

        // POUR PASSER D'UN WAYPOINT A UN AUTRE
        //if (agent.remainingDistance < .1f && !isWaiting)
        if (!agent.hasPath && !isWaiting)
        {
            isWaiting = true;

            // INVERSE
            if(!inverse)
            {
                index++;

                if(index >= waypoints.Count)
                    index = 0;

            }

            // NORMAL
            else
            {
                index--;
                if (index < 0)
                    index = waypoints.Count - 1;
            }

            if(stopAtWaypoint)
            {
                // COROUTINE WITH DELAY
                StartCoroutine(WaitForNextPos());
            }

            else
            {
                // SET DESTINATION
                agent.SetDestination(waypoints[index].position);
                isWaiting = false;
            }
            

        }

        if (agent.hasPath && isWaiting)
            isWaiting = false;
        


    }


    IEnumerator WaitForNextPos()
    {
        animator.SetTrigger("IDLE");
        yield return new WaitForSeconds(Random.Range(3f, 6f));

        agent.SetDestination(waypoints[index].position);
        animator.SetTrigger("WALK");
        
    }
}
