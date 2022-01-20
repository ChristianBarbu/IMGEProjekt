using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Hearth : MonoBehaviour
{
    public Sensor sensor;

    public float hearthValue = 0.2f;
    public float shieldValue = 0.1f;

    private void Start()
    {
        sensor.SensorTriggered.Subscribe(CollectSignalDetected).AddTo(this);
    }

    private void Collect()
    {
        var currentHealth = Mathf.Clamp(GameData.Instance.Health, 0, 1);
        var currentShield = Mathf.Clamp(GameData.Instance.Shield, 0, 1);

        if (currentHealth >= 1)
        {
            GameData.Instance.IncreaseShield(shieldValue);
        }
        else
        {
            GameData.Instance.IncreaseHealth(hearthValue);   
        }
    }

    void CollectSignalDetected(EventArgs args)
    {
        Collect();
        gameObject.SetActive(false);
        // why dont we delete it here?
    }
    
}
