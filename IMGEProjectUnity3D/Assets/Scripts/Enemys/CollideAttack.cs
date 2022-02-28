using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideAttack : EnemyAttack
{
    public override void Attack(Vector3 forward)
    {
        enemy.Hit();
        Destroy(gameObject);
    }
}
