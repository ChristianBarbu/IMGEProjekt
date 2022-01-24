using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{

    public float distanceToPlayer;

    public abstract void SetActive(bool b);

 
}
