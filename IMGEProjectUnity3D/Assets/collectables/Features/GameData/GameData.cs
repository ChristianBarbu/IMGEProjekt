using System;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : SingletonMonoBehaviour<GameData>
{
    private GameData()
    {
        EnemyCount = 0;
        CollectableCunt = 0;
        _difficultyScaling.Value = 1;
    }

    private ReactiveProperty<float> _difficultyScaling = new ReactiveProperty<float>();
    public ReactiveProperty<float> DifficultyScaling
    {
        get { return _difficultyScaling; }
        private set { _difficultyScaling = value; }
    }

    public int EnemyCount
    {
        get; set;
    }

    public int CollectableCunt
    {
        get; set;
    }

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
        if (Health <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
        Health = 0.5f;
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

    public ReactiveCollection<CompassBarElement> CompassBarElements { get; private set; } = new ReactiveCollection<CompassBarElement>();
}