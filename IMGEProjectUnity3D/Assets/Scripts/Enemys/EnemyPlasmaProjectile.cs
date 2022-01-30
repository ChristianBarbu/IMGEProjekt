using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlasmaProjectile: MonoBehaviour
{
    [Tooltip("max time until destroyed")]
    public float stayAlive = 5;

    public float homingSpeed;

    public AudioSource hitSound;
    public AudioSource moveSound;

    private float alivetime = 0;

    private Transform player;
    private Rigidbody rb;

    public Enemy Enemy { get; set; }    

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform; 
    }


    void Update()
    {
        alivetime += Time.deltaTime;
        if (alivetime > stayAlive)
            Destroy(gameObject);
        var vToPlayer = player.position +  - transform.position;

        Vector3 force = (vToPlayer).normalized * homingSpeed;
        force *= Time.deltaTime;
        rb.AddForce(force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Enemy?.Hit();
            if (hitSound != null)
            {
                hitSound.Play();
                Destroy(gameObject, hitSound.clip.length);
            }
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Enemy?.Hit();
            if (hitSound != null)
            {
                hitSound.Play();
                Destroy(gameObject, hitSound.clip.length);
            }
            else
                Destroy(gameObject);
        }
    }
    public void AddForce(float force)
    {
        var vToPlayer = GameObject.FindGameObjectWithTag("Player").transform.position - transform.position;
        rb.AddForce((vToPlayer).normalized * force);
    }
}
