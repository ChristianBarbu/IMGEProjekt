using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GameData : SingletonMonoBehaviour<GameData>
{
    private ReactiveProperty<int> score = new ReactiveProperty<int>(0);

    public event EventHandler<int> scoreUpdated;
    
    protected virtual void OnGameDataUpdate(int score)
    {
        scoreUpdated?.Invoke(this, score);
    }

    private void Update()
    {
        Debug.Log("score = " + score);
    }

    public int Score
    {
        get => score.Value;
        set
        {
            score.Value = value;
            OnGameDataUpdate(score.Value);
        }
    }

    public void IncreaseScore(int value)
    {
        Score += value;
    }

    public void ResetScore()
    {
        Score = 0;
    }
    
    
    
}
