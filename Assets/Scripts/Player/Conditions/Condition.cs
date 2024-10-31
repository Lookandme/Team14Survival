using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition : MonoBehaviour 
{
    public float maxValue;
    private float curValue;
    public float passiveValue;

    private void Start()
    {
        curValue = maxValue;
    }

    public void SetValue(float valueData)
    {
        maxValue= valueData;
        curValue = maxValue; 
    }

    public void Add(float value)
    {
        curValue = Mathf.Min(curValue + value,maxValue);
    }

    public void Substract(float value)
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