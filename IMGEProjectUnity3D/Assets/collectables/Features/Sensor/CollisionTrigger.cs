using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class CollisionTrigger : Sensor
{
    private void Awake()
    {
        //Debug.Log("Awake!");
        SensorTriggered = gameObject.AddComponent<ObservableCollisionTrigger>()
            .OnCollisionEnterAsObservable()
            .Where(col => col.collider.tag.Equals("Player"))
            .Select(e => new SensorEventArgs(e));
    }
}
