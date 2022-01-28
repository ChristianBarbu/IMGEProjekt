using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public Sensor sensor;

    public int value = 1;
    
    private void Start()
    {
        sensor.SensorTriggered.Subscribe(CollectSignalDetected).AddTo(this);
    }

    private void Collect()
    {
        GameData.Instance.IncreaseScore(value);
    }
    
    private void CollectSignalDetected(EventArgs args) {
        Collect();
        gameObject.SetActive(false);
        // why dont we delete it here?
    }

}
