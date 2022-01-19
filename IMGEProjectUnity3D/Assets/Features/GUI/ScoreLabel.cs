using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ScoreLabel : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    private void Start()
    {
        GameData.Instance.score
            .Subscribe(score => scoreText.text = "" + score)
            .AddTo(this);
    }

    private void Awake()
    {
        scoreText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if(scoreText == null) 
            Debug.Log("scoreText is null!");
    }

    void SetScoreText(object sender, int scoreValue)
    {
        scoreText.text = scoreValue.ToString();
    }

    private void OnEnable()
    {
        GameData.Instance.scoreUpdated += SetScoreText;
    }

    private void OnDisable()
    {
        GameData.Instance.scoreUpdated += SetScoreText;
    }
    
}
