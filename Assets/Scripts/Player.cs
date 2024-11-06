using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public PlayerController controller;

    public ItemData itemData;
    public Action addItem;
    public Equipment equip;
    public Transform dropPosition;

    public ConditionData playerData;
    public PlayerConditions condition;
    public PlayerDataBridge dataBridge;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        playerData = GetComponent<ConditionData>();
        condition = GetComponent<PlayerConditions>();
        equip = GetComponent<Equipment>();
        dataBridge = GetComponent<PlayerDataBridge>();
    }
}