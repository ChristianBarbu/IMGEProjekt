using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float targetDistanceToPlayer;
    public float attackRange;
    public float attackCoolDown;
    public EnemyAttack attack;

    public IObservable<bool> PlayerInRange { get; private set; }
    public ReactiveProperty<bool> OnCoolDown { get; private set; }
    public IObservable<bool> CanAttackPlayer { get; protected set; }

    protected float distanceToPlayer;
  

    protected GameObject player;
    private float cdWaited;


    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        CanAttackPlayer.Subscribe(b =>
        {
            if (b)
            {
                Attack();
                OnCoolDown.Value = true;
            }
        }).AddTo(this);
        this.UpdateAsObservable().Subscribe(_=>
        {
            if (OnCoolDown.Value)
            {
                cdWaited += Time.deltaTime;
                if (cdWaited >= attackCoolDown)
                {
                    OnCoolDown.Value = false;
                    cdWaited = 0;
                }
            }
            else
                cdWaited = 0;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), Time.deltaTime);
        }).AddTo(this);

    }


   

    public void Awake()
    {
        OnCoolDown = new ReactiveProperty<bool>();
        OnCoolDown.Value = false;


        PlayerInRange = this.UpdateAsObservable().Select(_ =>
        {
            if (player != null)
            {
                var r = (distanceToPlayer = (transform.position - player.transform.position).magnitude) < targetDistanceToPlayer;
                return r;
            }
            return false;
        }).AsObservable();

        CanAttackPlayer = Observable.CombineLatest(PlayerInRange,OnCoolDown.Select(b=>!b)).Select(l => !l.Any(b => !b));



    }



    public virtual void SetActive(bool b) { }
    public virtual void Attack() { attack?.Attack((player.transform.position - transform.position).normalized); }

 
}
