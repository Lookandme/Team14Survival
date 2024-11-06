using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    //싱글톤 선언 게임을 통틀어 저장을 담당해야함
    private static DataManager instance;
    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("DataManager").AddComponent<DataManager>();
            }
            return instance;
        }
    }

    public string path; //세이브파일의 폴더 경로
    public int nowSlot; // 현재 선택된 슬롯 정보

    private DataContainer container;
    public DataContainer Container
    {
        get { return container; }
        set{container = value;}
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);

        path = Application.persistentDataPath  + "data/data.json";
    }

    public void SaveData()
    {
        string data = JsonUtility.ToJson(container.playerDB); //클래스를 container에서 받아오도록 수정예정
        File.WriteAllText(path+nowSlot.ToString(),data);
    }

    public void LoadData()
    {
        string data = File.ReadAllText(path+nowSlot.ToString());
        container.playerDB = JsonUtility.FromJson<PlayerDB>(data);
    }

    public void DeleteData()
    {
        File.Delete(path+nowSlot.ToString()); //데이터 삭제 버튼에 붙여서 사용
    }

    public void DataClear() //선택 초기화
    {
        nowSlot = -1;
        container.playerDB = new PlayerDB();
    }
}