using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerDataBridge : MonoBehaviour
{
    public bool isFistRun = true;

    public float health;
    public float stamina;
    public float hunger;
    public float thirst;

    private void OnEnable()
    {
        CharacterManager.Instance.Player.dataBridge = this;
        if (DataManager.Instance.Container.playerDB.isFirst)
        {
            isFistRun=true;
        }
        else
        {
            isFistRun=false;
            ReceiveToDB();
        }

    }
    public void SendToDB()
    {
        GetConditionValue();
        GetPlayerPosition();
        DataManager.Instance.Container.playerDB.isFirst = false;
        DataManager.Instance.SaveData();
        Debug.Log("데이터 저장");
    }

    public void ReceiveToDB()
    {
        List<float> condition = DataManager.Instance.Container.playerDB.Conditions;
        SetConditionValue(condition);

        float x = DataManager.Instance.Container.playerDB.positionX; 
        float y = DataManager.Instance.Container.playerDB.positionY;
        float z = DataManager.Instance.Container.playerDB.positionZ;
        SetPlayerPosition(x, y, z);
    }

    private void GetConditionValue()
    {
        DataManager.Instance.Container.playerDB.Conditions = CharacterManager.Instance.Player.condition.SendConditionData(); //플레이어의 체력 등 현재 상태 값을 받아온 리스트를 저장 리스트의 리스트에 넣음
    }

    private void GetPlayerPosition()
    {
        DataManager.Instance.Container.playerDB.positionX = transform.position.x; //현재 오브젝트의 위치값을 저장
        DataManager.Instance.Container.playerDB.positionY = transform.position.y; //Vector3값은 JSON으로 변환할 수 없으므로 float값으로 나누어 저장
        DataManager.Instance.Container.playerDB.positionZ = transform.position.z;
    }

    private void SetPlayerPosition(float x,float y,float z)
    {
        transform.position = new Vector3(x, y, z); //저장된 위치값을 가져와 적용
    }

    private void SetConditionValue(List<float> condition)
    {
        health = condition[0];
        Debug.Log(health);
        stamina = condition[1];
        Debug.Log(stamina);
        hunger = condition[2];
        Debug.Log(hunger);
        thirst = condition[3];
        Debug.Log(thirst);
    }

}
