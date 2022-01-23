using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class CreditGenerator : MonoBehaviour, ICreditGenerator
{
    public float creditMultiplier = 1;

    public ReadOnlyReactiveProperty<double> Credits
    {
        get;
        private set;
    }


    private void Awake()
    {
        Credits = this.UpdateAsObservable()
            .Select(_ => 
            {
                return (creditMultiplier * (1.0 + GetDifficultyScaling())) / 2.0 * Time.deltaTime;
            })
            .ToReadOnlyReactiveProperty();
    }


    private double GetDifficultyScaling()
    {
        return GameData.Instance.DifficultyScaling.Value;
    }
}
