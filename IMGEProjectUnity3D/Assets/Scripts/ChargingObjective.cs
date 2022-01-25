using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class ChargingObjective: MonoBehaviour
{

    public float chargingRate = 1;
    public float dechargingRate = 1;
    public float leavingPenalty = 10;
    public float chargingGoal = 100;
    public Reward reward;

    public ReactiveProperty<bool> IsCharging { get; private set; }

    public ReactiveProperty<float> Charge { get; private set; }



    public IObservable<bool> Completed { get; protected set; }
    protected GameObject receiver;

    private void Awake()
    {
        IsCharging = new ReactiveProperty<bool>(false);
        Charge = new ReactiveProperty<float>();
        Completed = Charge.Select(c => c>=chargingGoal);

    }
    // Start is called before the first frame update
    void Start()
    {

        this.UpdateAsObservable().Subscribe(_ =>
        {
            if (IsCharging.Value)
                Charge.Value = Math.Min(chargingGoal,Charge.Value + chargingRate * Time.deltaTime);
            else
                Charge.Value = Math.Max(0, Charge.Value - dechargingRate * Time.deltaTime);
        }).AddTo(this);
        Completed.Where(c=>c).Subscribe(_ =>
        {
            reward.GiveReward(receiver);
            Destroy(gameObject);
        }).AddTo(this);
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
            Charge.Value = Math.Min(0, Charge.Value - 10);
            IsCharging.Value = false;
        }
    }
}
