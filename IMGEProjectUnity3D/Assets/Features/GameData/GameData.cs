using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GameData : SingletonMonoBehaviour<GameData>
{
    public ReactiveProperty<int> score = new ReactiveProperty<int>(0);
    
    public ReactiveProperty<float> health = new ReactiveProperty<float>(5);

    public ReactiveProperty<float> shield = new ReactiveProperty<float>(0);

    public event EventHandler<int> scoreUpdated;
    public event EventHandler<float> healthUpdated;
    public event EventHandler<float> shieldUpdated;

    protected virtual void OnGameDataUpdate(int score, float health, float shield)
    {
        scoreUpdated?.Invoke(this, score);
        healthUpdated?.Invoke(this, health);
        shieldUpdated?.Invoke(this, shield);
    }

    public int Score
    {
        get => score.Value;
        set
        {
            score.Value = value;
            OnGameDataUpdate(score.Value, health.Value, shield.Value);
        }
    }

    public float Health
    {
        get => health.Value;
        set
        {
            health.Value = value;
            OnGameDataUpdate(score.Value, health.Value, shield.Value);
        }
    }

    public float Shield
    {
        get => shield.Value;
        set
        {
            shield.Value = value;
            OnGameDataUpdate(score.Value, health.Value, shield.Value);
        }
    }

    public void IncreaseScore(int value)
    {
        Score += value;
    }

    public void IncreaseHealth(float value)
    {
        if (Health < 1)
            Health = Mathf.Clamp(Health + value, 0, 1);
    }

    public void DecreaseHealth(float value)
    {
        if (Health > 0)
            Health = Mathf.Clamp(Health - value, 0, 1);
    }

    public void DecreaseShield(float value)
    {
        if (Shield >= 0)
            Shield = Mathf.Clamp(Shield - value, 0, 1);
    }
    
    public void IncreaseShield(float value)
    {
        Shield = Mathf.Clamp(Shield + value, 0, 1);
    }

    public void ResetScore()
    {
        Score = 0;
    }

    public void ResetHealth()
    {
        Health = 1;
    }

    public void ResetShield()
    {
        Shield = 1;
    }
}