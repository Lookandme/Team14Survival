﻿using System;
using UnityEngine;

[Serializable]
public class ConditionDataValue
{
    public float MaxValue;

    public float passiveValue;
}

public class PlayerData : MonoBehaviour
{
    public ConditionDataValue health;
    public ConditionDataValue stamina;
    public ConditionDataValue hunger;
    public ConditionDataValue thirst;
}