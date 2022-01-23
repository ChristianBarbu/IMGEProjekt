using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySpawnCondition : EnemySpawnCondition
{
    public double upperDifficultyScalingThreshold;
    public double lowerDifficultyScalingThreshold;
    public override bool CanSpawn()
    {
        return GameData.Instance.DifficultyScaling.Value <= upperDifficultyScalingThreshold && GameData.Instance.DifficultyScaling.Value >= lowerDifficultyScalingThreshold;
    }
}
