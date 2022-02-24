using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
public abstract class Objective : MonoBehaviour
{

    public float progressGoal;

    public IReadOnlyReactiveProperty<bool> completed;
    public ReactiveProperty<float> progress {get; protected set;}

    public String objectiveTask;

    public abstract void Reward();

    public virtual void Awake()
    {
        progress = new ReactiveProperty<float>(0);
        completed = progress.Select(p =>
        {
            if (p >= progressGoal)
                return true;
            else 
                return false;
        }).ToReactiveProperty();
    }
    public virtual void Start()
    {
        // marker ??
        //marker = Instantiate(marker);
        //marker.target = gameObject.transform;
    }
}
