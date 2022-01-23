using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    public float difficultyScaling = 1;
    public float startCredits = 100;
    [Tooltip("Enemy is declared to cheap to spawn if credits >= cost * ToCheapMultiplier")]
    public float ToCheapMultiplier = 6;
    [Tooltip("Max enemies spawned in a short time frame")]
    public float MaxEnemysPerWave = 5;
    [Tooltip("Will not spawn if there are more enemys")]
    public int MaxEnemyCount = 40;

    [Tooltip("Inactive time after wave was spawned is determined by a random time bewteen minTimeBetweenWaves and maxTimeBetweenWaves")]
    public float minTimeBetweenWaves = 4;
    [Tooltip("Inactive time after wave was spawned is determined by a random time bewteen minTimeBetweenWaves and maxTimeBetweenWaves")]
    public float maxTimeBetweenWaves = 15;

    [Tooltip("Inactive time after a monster was spawned is determined by a random time bewteen minTimeBetweenSpawns and maxTimeBetweenWaves")]
    public float minTimeBetweenSpawns = 0.5f;
    [Tooltip("Inactive time after a monster was spawned is determined by a random time bewteen minTimeBetweenSpawns and maxTimeBetweenWaves")]
    public float maxTimeBetweenSpawns = 5;


    public GameObject player;
    public CreditGenerator creditGenerator;




    public List<GameObject> enemys;
    //public List<GameObject> bosses;

    private Dictionary<GameObject, EnemySpawnInfo> enemyData;

    private List<Collider> SpawnAreas;

    private double credits;
    private double combinedEnemyWeight;
    private double mostExpensiveCosts;


    private ReactiveProperty<double> waveTimerInterval = new ReactiveProperty<double>();
    private ReactiveProperty<double> spawnTimerInterval = new ReactiveProperty<double>();


    private void Awake()
    {
        enemyData=new Dictionary<GameObject, EnemySpawnInfo>(enemys.Select(e=> 
        {
            EnemySpawnInfo data = e.GetComponentInChildren<EnemySpawnInfo>();
            if (data == null)
                data = e.GetComponent<EnemySpawnCapsule>()?.enemy?.GetComponent<EnemySpawnInfo>();
            if(data.cost > mostExpensiveCosts)
                mostExpensiveCosts = data.cost;
            return new KeyValuePair<GameObject, EnemySpawnInfo>(e, data);
            }));
        combinedEnemyWeight = enemyData.Values.Sum(i => i.weight);
    }

    void Start()
    {
        credits = startCredits;
        creditGenerator.Credits
            .Subscribe(c => credits += c)
            .AddTo(this);
        SpawnAreas = new List<Collider>(GetComponentsInChildren<Collider>());

        // start loop
        StartCoroutine(SpawnLoop());

    }

    /// <summary>
    /// https://riskofrain2.fandom.com/wiki/Directors
    /// Spawn loop:
    ///     spawntimer = 0:
    ///         if overcrowding
    ///             skip
    ///         if previous spawn success:
    ///             increase spawn timer by spawn interval (during wave)
    ///             keep selected enemy
    ///         else
    ///             if preselected:
    ///                 prepare wave with preselected
    ///             else
    ///                 select random enemy
    ///             spawn counter = 0
    ///         check selected with:
    ///             - current wave <= 5 enemys
    ///             - spawn conditions
    ///             - affordable cost
    ///             - not too cheap:
    ///                 - credits > 6 * cost
    ///                 - not the most expensive (so it always spawns)
    ///             if all true:
    ///                 ;
    ///             else:
    ///                 spawn timer increas (between waves)
    ///                 current enemy deselected
    ///         prepare spawn
    ///         pay
    ///         spawncounter++
    ///         calculate hp/dmg
    ///         set reward
    ///         spawn
    /// </summary>
    void Update()
    {
    }

    private bool lastSpawnFailed = true;
    private GameObject selectedEnemy;
    private int spawnCounter = 0;

    IEnumerator SpawnLoop()
    {
        for(; ; )
        {
            if(IsOvercrowding())
            {
               yield return FailSpawn();
            }
            if (lastSpawnFailed)
            {
                if(selectedEnemy == null)
                {
                    SelectRandomEnemyByWeight();
                }
                spawnCounter = 0;
            }
            if (spawnCounter < MaxEnemysPerWave &&
                ConfirmSpawnConditions(selectedEnemy) &&
                IsAffordable(selectedEnemy) &&
                !IsToCheap(selectedEnemy))
            {
                //Spawn
                SpendCredits(selectedEnemy);
                spawnCounter++;
                // calculate hp/dmg
                // set reward
                var pos = CalculateSpawnPosition(selectedEnemy);
                if (pos.x == float.NegativeInfinity)
                    yield return FailSpawn();
                SpawnEnemy(selectedEnemy, pos);
                yield return SuccessSpawn();
            }
            else
            {
                yield return FailSpawn();
            }

        }
    }

    WaitForSeconds FailSpawn()
    {
        lastSpawnFailed = true;
        selectedEnemy = null;
        var t = GetWaveInactiveTime();
        Debug.Log("FAIL Spawn - WaitFor: " + t);
        return new WaitForSeconds(t);
    }
    WaitForSeconds SuccessSpawn()
    {
        lastSpawnFailed = false;
        var t = GetSpawnInactiveTime();
        Debug.Log("SUCCESS Spawn - WaitFor: " + t);
        return new WaitForSeconds(t);
    }

    void SpawnEnemy(GameObject enemy, Vector3 position)
    {
        Instantiate(enemy, position, new Quaternion());
    }

    Vector3 CalculateSpawnPosition(GameObject enemy)
    {
        var data = enemyData[enemy];
        UnityEngine.Random.Range(data.minSpawnDistance, data.maxSpawnDistance);
        var max = Physics.OverlapSphere(player.transform.position, data.maxSpawnDistance, (1 << gameObject.layer));
        var min = Physics.OverlapSphere(player.transform.position, data.minSpawnDistance, (1 << gameObject.layer));
        var selection = new List<Collider>(max.Except(min.Except(min.Intersect(max))));
        if (selection.Count == 0)
        {
            return new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
        }
        var area = selection[UnityEngine.Random.Range(0, selection.Count - 1)];
        var b = area.bounds;
        var relativePoint = new Vector3(
            UnityEngine.Random.Range(b.min.x, b.max.x),
            0,
            UnityEngine.Random.Range(b.min.z, b.max.z));
        Vector3 point = area.transform.position + relativePoint;
        var vToPlayer = point - player.transform.position;
        vToPlayer.y = 0;
        // Disregard spawn zones.. if spawn zones are placed equally it should work fine
        if (vToPlayer.magnitude < data.minSpawnDistance)
        {
            // To close shift backwards
            point = player.transform.position +  (vToPlayer.normalized * data.minSpawnDistance);
        }
        else if(vToPlayer.magnitude > data.maxSpawnDistance)
        {
            // to far shift forwards
            point = player.transform.position + (vToPlayer.normalized * data.maxSpawnDistance);
        }
        RaycastHit hit;
        Vector3 ceilingPoint = new Vector3(point.x, b.max.y, point.z);
        if (data.canSpawnInAir)
        {
            // find random point in air above mesh/ground
            Physics.Raycast(ceilingPoint, Vector3.down, out hit, b.size.y, ~gameObject.layer, QueryTriggerInteraction.Ignore);
            point.y = UnityEngine.Random.Range(hit.point.y, b.max.y);
        }
        else
        {
            // find point on mesh/ground
            if(Physics.Raycast(ceilingPoint, Vector3.down, out hit, b.size.y, ~gameObject.layer, QueryTriggerInteraction.Ignore))
                point.y = hit.point.y;
            else
            {
                // No ground found
                return new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
            }
        }
        return point;
    }

    

    void SelectRandomEnemyByWeight()
    {
        var v = UnityEngine.Random.value * combinedEnemyWeight;
        selectedEnemy = enemyData.FirstOrDefault(e => v >= (combinedEnemyWeight -= e.Value.weight)).Key;
    }

    bool IsToCheap(GameObject enemy)
    {
        var c = (float)enemyData[enemy].cost * ToCheapMultiplier <= credits && enemyData[enemy].cost != mostExpensiveCosts;
        if (c)
            Debug.Log("FAIL - To Cheap");
        return c;
    }

    bool IsAffordable(GameObject enemy)
    {
        var c = enemyData[enemy].cost <= credits;
        if (!c)
            Debug.Log("FAIL - Not Affordable");
        return c;
    }

    bool ConfirmSpawnConditions(GameObject enemy)
    {
        var c = !enemyData[enemy].conditions.Any(c => !c.CanSpawn());
        if(!c)
            Debug.Log("FAIL - Condition");
        return c;
    }

    void SpendCredits(GameObject enemy) 
    {
        credits -= enemyData[enemy].cost;
    }

    bool IsOvercrowding()
    {
        var c = GameData.Instance.EnemyCount >= MaxEnemyCount;
        if (c)
            Debug.Log("FAIL - Overcrowding");
        return c;
    }

    float GetWaveInactiveTime()
    {
        return UnityEngine.Random.RandomRange(minTimeBetweenWaves, maxTimeBetweenWaves);
    }
    float GetSpawnInactiveTime()
    {
        return UnityEngine.Random.RandomRange(minTimeBetweenSpawns, maxTimeBetweenSpawns);
    }
}
