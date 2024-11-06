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
    public GameObject createUI; //새로 작성시 인풋필드
    public GameObject controlPanel; //불러오기 삭제 UI
    public TextMeshProUGUI[] slotText; //슬롯버튼 UI 텍스트
    public TextMeshProUGUI newPlayer;  //새로 이름 작성시 닉네임 출력

    bool[] savefile = new bool[3];

    private void Start()
    {
        DataManager dataManager = DataManager.Instance;
        for (int i = 0;  i < savefile.Length; i++) 
        {
            if(File.Exists(dataManager.path + $"{i}"))//세 개의 데이터 파일에 값이 있는지 검사(데이터 값은 모두 숫자 i로 판별됨)
            {
                savefile[i] = true;
                dataManager.nowSlot = i;
                dataManager.LoadData();
                slotText[i].text = dataManager.Container.playerDB.name;
            }
            else
            {
                slotText[i].text = "비어있음";
            }
        }
        dataManager.DataClear(); //변수 초기화 
    }

    public void Slot(int num)
    {
        DataManager.Instance.nowSlot = num; //클릭한 슬롯의 번호를 경로의 번호로 입력

        if (!savefile[num])//만약 슬롯 넘버에 해당하는 세이브파일이 없다면
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
        slotText[DataManager.Instance.nowSlot].text = "비어있음";
        DataManager.Instance.DataClear(); // 삭제 후엔 슬롯 초기화
        controlPanel.SetActive(false);
    }

    public void StartGame()
    {
        DataManager.Instance.LoadData();
        LoadGame();
    }
    
    public void Exit() //x버튼
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
