using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Select : MonoBehaviour
{
    public GameObject createUI; //���� �ۼ��� ��ǲ�ʵ�
    public GameObject controlPanel; //�ҷ����� ���� UI
    public TextMeshProUGUI[] slotText; //���Թ�ư UI �ؽ�Ʈ
    public TextMeshProUGUI newPlayer;  //���� �̸� �ۼ��� �г��� ���

    bool[] savefile = new bool[3];

    private void Start()
    {
        DataManager dataManager = DataManager.Instance;
        for (int i = 0;  i < savefile.Length; i++) 
        {
            if(File.Exists(dataManager.path + $"{i}"))//�� ���� ������ ���Ͽ� ���� �ִ��� �˻�(������ ���� ��� ���� i�� �Ǻ���)
            {
                savefile[i] = true;
                dataManager.nowSlot = i;
                dataManager.LoadData();
                slotText[i].text = dataManager.Container.playerDB.name;
            }
            else
            {
                slotText[i].text = "�������";
            }
        }
        dataManager.DataClear(); //���� �ʱ�ȭ 
    }

    public void Slot(int num)
    {
        DataManager.Instance.nowSlot = num; //Ŭ���� ������ ��ȣ�� ����� ��ȣ�� �Է�

        if (!savefile[num])//���� ���� �ѹ��� �ش��ϴ� ���̺������� ���ٸ�
        {
            Creat();
        }
        else
        {
            LoadOrDelete();
        }
    }

    private void Creat()
    {
        createUI.gameObject.SetActive(true);
    }

    private void LoadOrDelete()
    {
        controlPanel.SetActive(true);
    }

    public void Delete()
    {
        DataManager.Instance.DeleteData();
        slotText[DataManager.Instance.nowSlot].text = "�������";
        DataManager.Instance.DataClear(); // ���� �Ŀ� ���� �ʱ�ȭ
        controlPanel.SetActive(false);
    }

    public void StartGame()
    {
        DataManager.Instance.LoadData();
        LoadGame();
    }
    
    public void Exit() //x��ư
    {
        controlPanel.gameObject.SetActive(false);
    }

    public void LoadGame()
    {
        if (!savefile[DataManager.Instance.nowSlot])
        {
            DataManager.Instance.Container.playerDB.name = newPlayer.text;
            DataManager.Instance.SaveData();
        }
        SceneManager.LoadScene("MainScene");
    }
}
