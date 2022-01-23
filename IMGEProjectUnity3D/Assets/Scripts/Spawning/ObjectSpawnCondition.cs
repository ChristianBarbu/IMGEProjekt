using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnCondition : MonoBehaviour
{
    public virtual bool CanSpawn()
    {
        return true;
    }
}
