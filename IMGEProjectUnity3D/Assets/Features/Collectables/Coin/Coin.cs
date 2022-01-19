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

    public void Collect()
    {
        GameData.Instance.IncreaseScore(value);
    }
    
    void CollectSignalDetected(EventArgs args) {
        Debug.Log("CollectSignalDetected!");
        Collect();
        gameObject.SetActive(false);
    }

}
