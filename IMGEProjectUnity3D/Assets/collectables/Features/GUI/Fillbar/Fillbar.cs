using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class Fillbar : MonoBehaviour
{
    private Image barImage;
    private Sensor sensor;

    public enum BarType
    {
        Healthbar,
        Shieldbar
    }

    public BarType bartype;

    private void Awake()
    {
        barImage = gameObject.GetComponent<Image>();
    }
    
    private void Start()
    {
        ReactiveProperty<float> bar = null;

        switch (bartype)
        {
            case BarType.Healthbar:
                bar = GameData.Instance.health;
                break;
            case BarType.Shieldbar:
                bar = GameData.Instance.shield;
                break;
        }

        bar.Subscribe(barFillValue => barImage.fillAmount = barFillValue)
            .AddTo(this);
        
    }

    void SetFillAmount(object sender, float fillAmount)
    {
        barImage.fillAmount = fillAmount;
    }

    private void OnEnable()
    {
        switch (bartype)
        {
            case BarType.Healthbar:
                GameData.Instance.healthUpdated += SetFillAmount;
                return;
            case BarType.Shieldbar:
                GameData.Instance.shieldUpdated += SetFillAmount;
                break;
        }
    }

    private void OnDisable()
    {
        switch (bartype)
        {
            case BarType.Healthbar:
                GameData.Instance.healthUpdated -= SetFillAmount;
                return;
            case BarType.Shieldbar:
                GameData.Instance.shieldUpdated -= SetFillAmount;
                break;
        }
    }
    
}