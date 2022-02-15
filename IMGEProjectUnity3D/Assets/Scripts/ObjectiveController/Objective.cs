using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
public abstract class Objective : MonoBehaviour
{
    public ReactiveProperty<bool> completed;
    public ReactiveProperty<float> progress {get; protected set;}
    public int progressGoal;

    protected CompassBarElement marker;

    public abstract void Reward();


}
