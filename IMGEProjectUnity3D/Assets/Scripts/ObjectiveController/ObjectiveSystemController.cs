using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class ObjectiveSystemController : MonoBehaviour
{
    public Objective[] Objectives;
    private Objective currentObjective;
    //private ReactiveProperty<bool> curObjCompleted {get; set;}


    private void Awake()
    {
    }

    public void Start()
    {
        currentObjective = Objectives[UnityEngine.Random.Range(0, Objectives.Length)];



    }








}
