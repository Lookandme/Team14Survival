using System;
using UnityEngine;
using UnityEngine.UI;


public enum ConditionType
{
    health,
    stamina,
    hunger,
    thirst
}

public class ConditionUI : MonoBehaviour
{
    [SerializeField]
    private PlayerConditions conditions;

    [SerializeField] ConditionType type;

    private Image valueBar;

    private float filledUIValue;
    

    private void Start()
    {
        conditions = CharacterManager.Instance.Player.condition;
        valueBar = GetComponent<Image>();
    }

    private void Update()
    {
        SetFilledValue();

        if ( valueBar != null)
        {
            valueBar.fillAmount = filledUIValue;
        }
    }

    private void SetFilledValue()
    {
        switch (type)
        {
            case ConditionType.health:
                filledUIValue = conditions.health.GetPercentage();
                break;
            case ConditionType.stamina:
                filledUIValue = conditions.stamina.GetPercentage();
                break;
            case ConditionType.hunger:
                filledUIValue = conditions.hunger.GetPercentage();
                break;
            case ConditionType.thirst:
                filledUIValue = conditions.thirst.GetPercentage();
                break;
        }
    }
}