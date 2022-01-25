using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleObjectiveSpawnController : ObjectSpawnController
{

    protected override bool IsOvercrowding()
    {
        return spawnedObject != null;
    }
}
