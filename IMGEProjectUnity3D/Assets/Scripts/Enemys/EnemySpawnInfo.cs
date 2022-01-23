using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnInfo : MonoBehaviour
{
    [Tooltip("spawn cost")]
    public int cost;

    [Tooltip("chance to be selected")]
    public float weight;

    [Tooltip("Minimum spawn distance to player")]
    public float minSpawnDistance;

    [Tooltip("Maximum spawn distance to player")]
    public float maxSpawnDistance;

    public bool canSpawnInAir;

    public List<EnemySpawnCondition> conditions = new List<EnemySpawnCondition>();
}
