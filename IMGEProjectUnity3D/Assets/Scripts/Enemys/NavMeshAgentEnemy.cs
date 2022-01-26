using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentEnemy : Enemy
{
    NavMeshAgent agent;


    public override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
    }


    private void Update()
    {
        if (!agent.isOnNavMesh)
            return;
        if (distanceToPlayer <= targetDistanceToPlayer)
        {
            agent.isStopped = true;
            return;
        }
        else
        {
            agent.isStopped = false;
        }
        RaycastHit hit;
        if(Physics.Raycast(player.transform.position,Vector3.down,out hit))
        {
            NavMeshHit navMeshHit;
            NavMesh.SamplePosition(hit.point, out navMeshHit, Mathf.Max(100,this.distanceToPlayer), 1);
            if (navMeshHit.hit)
            {
                var preDest = agent.destination;
                //agent.SetDestination(navMeshHit.position);
                agent.CalculatePath(navMeshHit.position, agent.path);
                if (agent.pathStatus != NavMeshPathStatus.PathComplete)
                {
                    agent.SetDestination(preDest);  
                }
                else
                {
                    agent.destination = navMeshHit.position;
                }

            }
        }
    }

    public override void SetActive(bool b)
    {
        if(agent == null)
            agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
    }


}
