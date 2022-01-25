using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Sensor sensor;
    public float playerHealthDecrease;
    public float playerShieldDecrese;

    #region degbug freeze
    public float speed;
    public float width;
    public float zwidth;

    private Vector3 curPos;
    
    private float timeCounter = 0;
    
    private void Update()
    {
        timeCounter += Time.deltaTime*speed;

        float x = Mathf.Cos(timeCounter)*width + curPos.x;
        float y = curPos.y;
        float z = Mathf.Sin(timeCounter)*zwidth + curPos.z;

        transform.position = new Vector3(x, y, z);
    }
    #endregion
    private void Start()
    {
        curPos = transform.position;
        sensor.SensorTriggered.Subscribe(HitSignalDetected).AddTo(this);
    }

    public void Hit()
    {
        var currentHealth = Mathf.Clamp(GameData.Instance.Health, 0, 1);
        var currentShield = Mathf.Clamp(GameData.Instance.Shield, 0, 1);

        Debug.Log("currentHealth = " + currentHealth);
        Debug.Log("currentShield = " + currentShield);
        
        if (currentHealth >= 1 && currentShield > 0)
        {
            GameData.Instance.DecreaseShield(playerShieldDecrese);
        }
        else
        {
            GameData.Instance.DecreaseHealth(playerHealthDecrease); 
        }
    }

    void HitSignalDetected(EventArgs args)
    {
        Hit();
        gameObject.SetActive(false);
        // why dont we delete it here?
    }
    
}
