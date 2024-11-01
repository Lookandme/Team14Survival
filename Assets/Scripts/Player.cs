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
    public PlayerCoditions condition;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCoditions>();
        equip = GetComponent<Equipment>();
    }




}
