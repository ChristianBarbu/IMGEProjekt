using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Freeze : MonoBehaviour
{
    public Sensor sensor;
    public float freezeTimeInSec = 3;

    // nicer solution should be found here
    public GameObject snowflake;
    
    public List<Enemy> _enemies;

    /**
     * Adds an enemy to the freeze list, i.e. every enemy on the freeze list gets froze if the freeze collectable has been collected
     *
     *  (approach might be garbage, using it as a stub here)
     */
    public void AddEnemyToFreezeList(Enemy enemy)
    {
        _enemies.Add(enemy);
    }
    
    private void Start()
    {
        sensor.SensorTriggered.Subscribe(CollectSignalDetected).AddTo(this);
    }

    private IEnumerator Collect()
    {
        float curSpeed = 0;
        if (_enemies.Count > 0)
            curSpeed = _enemies[0].Speed;
        
        foreach (var enemy in _enemies)
        {
            enemy.Speed = 0;
        }

        Debug.Log("Time.timeScale = " + Time.timeScale);
        
        yield return new WaitForSeconds(freezeTimeInSec);

        foreach (var enemy in _enemies)
        {
            enemy.Speed = curSpeed;
        }
        
        // destroy or deactivate parent of snowflake
        Destroy(gameObject);
        
    }

    public void CollectSignalDetected(EventArgs args)
    {
        StartCoroutine(Collect());
        snowflake.SetActive(false);
        transform.GetComponent<BoxCollider>().enabled = false;
    }

}
