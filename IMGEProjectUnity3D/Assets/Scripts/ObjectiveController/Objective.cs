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


    protected CompassBarElement marker;

    public abstract void Reward();

    public virtual void Awake()
    {
        progress = new ReactiveProperty<float>();
        completed = progress.Where(p => p >= progressGoal).Select(_=>true).ToReactiveProperty();
    }
    public virtual void Start()
    {
      // marker ??
    }


}
