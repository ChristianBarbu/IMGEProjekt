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

    public ProgressBarController progressBar;

    public SingleObjectiveSpawnController spawner;
    private IReadOnlyReactiveProperty<bool> curObjCompleted {get; set;}


    private void Awake()
    {
        
    }

    private void Start()
    {
        createRandomObjective();
        curObjCompleted = currentObjective.completed.ToReactiveProperty();
        curObjCompleted.Where(completed => completed == true).Subscribe(_ =>
        {
            Destroy(currentObjective);
            createRandomObjective();
            curObjCompleted = currentObjective.completed.ToReactiveProperty();
        }).AddTo(this);
    }

    private void createRandomObjective()
    {
        currentObjective = spawner.SpawnObject(Objectives[UnityEngine.Random.Range(0, Objectives.Length)].gameObject).GetComponent<Objective>();
        marker.target = currentObjective.transform;
        progressBar.textObject.text = currentObjective.objectiveTask;
        progressBar.slider.maxValue = currentObjective.progressGoal;
        currentObjective.progress.Subscribe(value =>
        {
            progressBar.slider.value = value;
        }).AddTo(this);
    }









}
