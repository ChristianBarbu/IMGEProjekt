using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CompassBarElement : MonoBehaviour
{
    [SerializeField] private Transform player;
    public Transform target;
    [SerializeField] private bool useFixDirection = false;
    [SerializeField] private Vector3 fixDirection;


    public CompassBar Bar { get; set; }
    private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        Bar = GetComponentInParent<CompassBar>();
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        GameData.Instance.CompassBarElements.Add(this);
    }

    private void Update()
    {
        if (Bar != null)
        {
            var u = -Bar.BarRectTransform.rect.width / 90;
            var max = Bar.BarRectTransform.rect.width / 2;
            float xPosition = 0;
            if (!useFixDirection)
            {
                if (target is not null)
                {
                    Vector3 dir = (target.position - player.position).normalized;
                    Vector2 d = new Vector2(dir.x, dir.z);
                    float angle = Vector2.SignedAngle(new Vector2(player.forward.x, player.forward.z), d);
                    xPosition = u * angle;
                    //float xPosition = angle * (360 / bar.BarRange) * (bar.BarRectTransform.rect.width / 2);
                    _rectTransform.anchoredPosition = new Vector2(Mathf.Clamp(xPosition,-max+15,max-15), 0);
                }
            }
            else
            {
                float angle = Vector2.SignedAngle(new Vector2(player.forward.x, player.forward.z), new Vector2(fixDirection.x, fixDirection.z));
                xPosition = u * angle;
                _rectTransform.anchoredPosition = new Vector2(xPosition, 0);
            }
         
        }
    }
    public void Remove()
    {
        GameData.Instance.CompassBarElements.Remove(this);
    }
}