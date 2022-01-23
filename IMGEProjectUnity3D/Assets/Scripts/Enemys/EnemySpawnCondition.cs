using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnCondition : MonoBehaviour
{
    public virtual bool CanSpawn()
    {
        return true;
    }
}
