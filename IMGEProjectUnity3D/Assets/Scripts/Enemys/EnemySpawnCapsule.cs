using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawnCapsule : MonoBehaviour
{
    public GameObject enemy;
    public GameObject capsule;
    public GameObject enemyContainer;
    public Animator animator;


    private void Awake()
    {
        if (capsule == null)
            capsule = new GameObject();
    }

    // Start is called before the first frame update
    void Start()
    {
        enemy = Object.Instantiate(enemy, enemyContainer.transform);
        var ai = enemy.GetComponent<Enemy>();
        ai.SetActive(false);
        ScaleCapsuleToEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Bounds GetMaxBounds(GameObject g)
    {
        var renderers = g.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return new Bounds(g.transform.position, Vector3.zero);
        var b = renderers[0].bounds;
        foreach (Renderer r in renderers)
        {
            b.Encapsulate(r.bounds);
        }
        return b;
    }
    void ScaleCapsuleToEnemy()
    {
        if (enemy == null)
            return;
        var b = GetMaxBounds(enemy);
        var size = b.size;
        size.Scale(new Vector3(1, 0.5f, 1));
        size.Scale(new Vector3(1.2f, 1.3f, 1.2f));
        capsule.transform.localScale = size;
    }
    public void ReparentEnemy()
    {
        enemy.transform.SetParent(transform.parent);
    }
    public void Dissolve()
    {
        var ai = enemy.GetComponent<Enemy>();
        ai.SetActive(true);
        Destroy(gameObject);
    }

    
}
