using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour
{
    protected Transform player;

    public Enemy Enemy { get; set; }

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    public abstract void Attack(Vector3 forward);
}
