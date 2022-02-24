using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class ObjectiveSystemController : MonoBehaviour
{
    public Objective[] Objectives;
    private Objective currentObjective;
    public CompassBarElement marker;
    private IReadOnlyReactiveProperty<bool> curObjCompleted {get; set;}


    private void Awake()
    {
        createRandomObjective();
        curObjCompleted = currentObjective.completed.ToReactiveProperty();
    }

    private void Start()
    {
        curObjCompleted.Where(completed => true).Subscribe(_ =>
        {
            Destroy(currentObjective);
            createRandomObjective();
            curObjCompleted = currentObjective.completed.ToReactiveProperty();
        }).AddTo(this);
    }

    private void createRandomObjective()
    {
        currentObjective = Objectives[UnityEngine.Random.Range(0, Objectives.Length)];
        Instantiate(currentObjective);
        marker.target = currentObjective.transform;
    }









}
