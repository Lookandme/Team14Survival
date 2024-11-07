using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;

    public ItemData itemData;
    public Action addItem;
    public Equipment equip;
    public Transform dropPosition;

    public PlayerData playerData;
    public PlayerConditions condition;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        playerData = GetComponent<PlayerData>();
        condition = GetComponent<PlayerConditions>();
        equip = GetComponent<Equipment>();
    }

}
