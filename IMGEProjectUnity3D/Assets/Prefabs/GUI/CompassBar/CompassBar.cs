using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class CompassBar : MonoBehaviour
{
    public float BarRange => barRange;
    
    [SerializeField] private float barRange = 120;

    public RectTransform BarRectTransform => _rectTransform;
    private RectTransform _rectTransform;

    private Transform childrenHost;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        childrenHost = transform.GetChild(0);
    }
    private void Start()
    {
        GameData.Instance.CompassBarElements.ObserveAdd().Subscribe((o) =>
        {
            o.Value.transform.parent = childrenHost.transform;
            o.Value.Bar = this;
        }).AddTo(this);
        GameData.Instance.CompassBarElements.ObserveRemove().Subscribe((o) =>
        {
            o.Value.gameObject.SetActive(false);
            Destroy(o.Value.gameObject);
        }).AddTo(this);
    }
}