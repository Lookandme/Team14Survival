using UnityEngine;
using UnityEngine.UI;

public class ConditionUI : MonoBehaviour
{
    [SerializeField]
    protected UIConditionData data;

    protected float filledUIValue = 1.0f;
    protected Image valueBar;

    protected virtual void Start()
    {
        data = GetComponentInParent<UIConditionData>();
    }

    protected virtual void Update()
    {
        valueBar.fillAmount = filledUIValue;
    }
}