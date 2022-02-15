using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveSystemController : MonoBehaviour
{
    public GameObject[] Objectives;
    private GameObject currentObjective;
    


    public void Start()
    {
        currentObjective = Objectives[Random.Range(0, Objectives.Length + 1)];
    }








}
