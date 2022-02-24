using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
public abstract class Objective : MonoBehaviour
{

    public int progressGoal;

    public IReadOnlyReactiveProperty<bool> completed;
    public ReactiveProperty<float> progress {get; protected set;}

    public abstract void Reward();

    public virtual void Awake()
    {
        progress = new ReactiveProperty<float>();
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
