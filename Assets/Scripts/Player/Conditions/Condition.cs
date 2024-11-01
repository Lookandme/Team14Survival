using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition : MonoBehaviour 
{
    public float maxValue;
    private float curValue;
    public float passiveValue;

    public void SetValue(float valueData,float passiveValueData)
    {
        maxValue= valueData;
        curValue = maxValue;
        passiveValue = passiveValueData;
    }

    public void Add(float value)
    {
        curValue = Mathf.Min(curValue + value,maxValue);
    }

    public void Subtract(float value)
    {
        curValue = Mathf.Max(curValue - value,0);
    }

    public float GetValue()
    {
        return curValue;
    }

    public float GetPercentage()
    {
        return curValue / maxValue;
    }
}