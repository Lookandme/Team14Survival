using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    //�̱��� ���� ������ ��Ʋ�� ������ ����ؾ���
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

    public string path; //���̺������� ���� ���
    public int nowSlot; // ���� ���õ� ���� ����

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
        string data = JsonUtility.ToJson(container.playerDB); //Ŭ������ container���� �޾ƿ����� ��������
        File.WriteAllText(path+nowSlot.ToString(),data);
    }

    public void LoadData()
    {
        string data = File.ReadAllText(path+nowSlot.ToString());
        container.playerDB = JsonUtility.FromJson<PlayerDB>(data);
    }

    public void DeleteData()
    {
        File.Delete(path+nowSlot.ToString()); //������ ���� ��ư�� �ٿ��� ���
    }

    public void DataClear() //���� �ʱ�ȭ
    {
        nowSlot = -1;
        container.playerDB = new PlayerDB();
    }
}