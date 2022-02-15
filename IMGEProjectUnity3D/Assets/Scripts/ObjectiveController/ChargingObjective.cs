using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class ChargingObjective: Objective
{

    public float chargingRate = 1;
    public float dechargingRate = 1;
    public float leavingPenalty = 10;
    public Reward reward;

    public ReactiveProperty<bool> IsCharging { get; private set; }

    //public ReactiveProperty<float> Charge { get; private set; }

    //public IObservable<bool> Completed { get; protected set; }
    protected GameObject receiver;



    private void Awake()
    {
        progressGoal = 100;

        IsCharging = new ReactiveProperty<bool>(false);
        progress = new ReactiveProperty<float>();
        completed = new ReactiveProperty<bool>(false);
    }
    // Start is called before the first frame update
    void Start()
    {

        this.UpdateAsObservable().Subscribe(_ =>
        {
            if (IsCharging.Value)
                progress.Value = Math.Min(progressGoal,progress.Value + chargingRate * Time.deltaTime);
            else
                progress.Value = Math.Max(0, progress.Value - dechargingRate * Time.deltaTime);
        }).AddTo(this);
        completed.Where(c=>c).Subscribe(_ =>
        {
            reward.GiveReward(receiver);
            marker.Remove();
            Destroy(gameObject);
        }).AddTo(this);
        marker = Instantiate(marker);
        marker.target = gameObject.transform;
    }

    
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            IsCharging.Value = true;
            receiver = other.gameObject;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            progress.Value = Math.Min(0, progress.Value - 10);
            IsCharging.Value = false;
        }
    }
    public override void Reward()
    {
        
    }
}
