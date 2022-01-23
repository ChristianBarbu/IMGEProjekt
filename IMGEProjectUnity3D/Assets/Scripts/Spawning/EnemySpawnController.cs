using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : ObjectSpawnController
{
    protected override bool IsOvercrowding()
    {
        var c = GameData.Instance.EnemyCount >= MaxObjectsCount;
        if (c)
            Debug.Log("FAIL - Overcrowding");
        return c;
    }
}
