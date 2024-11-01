using UnityEngine;
using UnityEngine.UI;

public class ConditionUI : MonoBehaviour
{
    [SerializeField]
    protected UIConditionData data;

    protected float filledUIValue;
    protected Image valueBar;

    protected virtual void Start()
    {
        data = GetComponentInParent<UIConditionData>();
        valueBar = GetComponent<Image>();
    }

    protected virtual void Update()
    {
        if (valueBar != null)
        {
            valueBar.fillAmount = filledUIValue;
        }
    }
}