using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentEnemy : Enemy
{
    NavMeshAgent agent;
    protected GameObject player;

    void Start()
    {   
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!agent.isOnNavMesh)
            return;
        if ((transform.position - player.transform.position).magnitude <= distanceToPlayer)
        {
            agent.isStopped = true;
            return;
        }
        RaycastHit hit;
        if(Physics.Raycast(player.transform.position,Vector3.down,out hit))
        {
            NavMeshHit navMeshHit;
            NavMesh.SamplePosition(hit.point, out navMeshHit, Mathf.Max(100,this.distanceToPlayer), 1);
            if (navMeshHit.hit)
                agent.SetDestination(navMeshHit.position);
        }
    }
}
