using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

public class ResourcePool : MonoBehaviour
{
    public Dictionary<string, Queue<GameObject>> resourceDictionary = new();

    public void SetPool(string resourceName,List<GameObject> resourceList) 
    {
        Queue<GameObject> resourceQueue = new Queue<GameObject>(); //����Ʈ�� ������ ���� ť ����
        for(int i = 0; i< resourceList.Count; i++)
        {
            resourceQueue.Enqueue(resourceList[i]); //����Ʈ�� ������Ʈ�� ť�� Enqueue
        }
        resourceDictionary.Add(resourceName, resourceQueue);    //��ųʸ��� �ش� �������� �̸��� ������Ʈ�� ���� ť�� ���
    }

    public GameObject GetResourceInPool(string resourceName)
    {
        if (!resourceDictionary.ContainsKey(resourceName)) return null; //�ش� �̸��� Ű�� ���ٸ� ���ư�

        if (resourceDictionary[resourceName] == null) return null; 
        GameObject obj = resourceDictionary[resourceName].Dequeue();
        return obj;
    }

    public bool CheckQueueEmpty(string name) 
    {
        if (!resourceDictionary.ContainsKey(name)) return false; // Ű�� ���� ��� false

        bool isIn = resourceDictionary[name].TryDequeue(out GameObject obj); //TryDequeue�� Dequeue�� �������� Ȯ���Ѵ�.
        if (!isIn) return false;  //Dequeue�� �Ұ��� false
        resourceDictionary[name].Enqueue(obj);//Dequeue�� �Ǿ��� �� �ٽ� ���� ������Ʈ�� ����־��ش�.
        return isIn; //true�� ��ȯ
    }

    public void GetUselessResource()
    {
        Resource[] objects = GetComponentsInChildren<Resource>(true); //������Ʈ�� ���� ���ӿ�����Ʈ���� ������ true�� �� ������ ��Ȱ��ȭ �� ������Ʈ�� ã�� ����
        foreach (Resource obj in objects)
        {
            if(obj.GetComponent<Resource>().capacity == 0 && !obj.gameObject.activeSelf) //�ش� ������Ʈ�� ���ҽ� ������Ʈ ������ ������ ü���� 0�̰� ��Ȱ��ȭ �� ���� ã��
            {
                obj.GetComponent<Resource>().capacity = 10; //������Ʈ�� ü���� ���� ����

                string name = obj.name;
                name = name.Replace("(Clone)","");//Ű�� �Է��ϱ� ���� ������ �������� �̸��� ������ clone�� ����
                Debug.Log(name);

                resourceDictionary[name].Enqueue(obj.gameObject) ; //�ش������Ʈ�� �±׿� �´� Ű ���� �Է�
            }
        }
    }
}
