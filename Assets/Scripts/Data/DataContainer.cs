using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerDB
{
    public string name; //데이터 파일 이름
    public List<float> Conditions; //플레이어 현 상태 값
    public float positionX; //플레이어 현위치의 x값
    public float positionY; //플레이어 현위치의 Y값
    public float positionZ; //플레이어 현위치의 Z값

    public bool isFirst = true;
}

[Serializable]
public class InventoryDB
{
    //인벤토리는 양이 많으므로 따로 저장
}

public class DataContainer : MonoBehaviour
{
    public PlayerDB playerDB = new PlayerDB();

    private void Awake()
    {
        DataManager.Instance.Container = this;
    }

}