using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentEnemy : Enemy
{
    NavMeshAgent agent;
    private int skipped = 0;
    
    public override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
    }

    public override float Speed
    {
        get { return speed;}
        set
        {
            speed = value;
            agent.speed = value;
        }
    }

    private void Update()
    {
        if (!agent.isOnNavMesh)
        {
            skipped++;
            if(skipped > 3) 
            {
                NavMeshHit h;
                if(NavMesh.SamplePosition(agent.transform.position, out h, 20, NavMesh.AllAreas))
                {
                    transform.position = h.position + new Vector3(0, agent.baseOffset, 0);
                }
            }
            agent.enabled = false;
            Invoke(nameof(EnableAgent), 0.5f);
            return;
            //NavMeshHit h;
            //if(NavMesh.SamplePosition(agent.transform.position, out h, 1, 1))
            //{
            //    agent.transform.position = h.position;
            //    agent.enabled = false;
               
            //    return;
            //}
            //else
            //{
            //    StartCoroutine(Wait(2));
            //    return;
            //}
        }
        skipped = 0;
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

    void EnableAgent()
    {
        agent.enabled = true;
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }

    public override void SetActive(bool b)
    {
        if(agent == null)
            agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
    }


}
