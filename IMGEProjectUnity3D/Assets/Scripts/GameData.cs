using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GameData
{
    private GameData()
    {
        EnemyCount = 0;
        CollectableCunt = 0;
        _difficultyScaling.Value = 1;
    }

    private static GameData _instance;
    public static GameData Instance 
    {
        get 
        { 
            return _instance ?? (_instance = new GameData()); 
        }
    }

    private ReactiveProperty<float> _difficultyScaling = new ReactiveProperty<float>();
    public ReactiveProperty<float> DifficultyScaling
    {
        get { return _difficultyScaling; }
        private set { _difficultyScaling = value; }
    }

    public int EnemyCount
    {
        get;set;
    }

    public int CollectableCunt
    {
        get;set;
    }

}
