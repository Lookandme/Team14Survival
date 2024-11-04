using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

public class ResourcePool : MonoBehaviour
{
    public Dictionary<string, Queue<GameObject>> resourceDictionary = new();

    public void SetPool(string resourceName,List<GameObject> resourceList) //���丮���� ������ ������Ʈ���� ť�� ��� ��ųʸ��� �������
    {
        Queue<GameObject> resourceQueue = new Queue<GameObject>();
        for(int i = 0; i< resourceList.Count; i++)
        {
            resourceQueue.Enqueue(resourceList[i]);
        }
        resourceDictionary.Add(resourceName, resourceQueue);   
    }

    public GameObject GetResourceInPool(string resourceName)
    {
        if (!resourceDictionary.ContainsKey(resourceName)) return null;

        if (resourceDictionary[resourceName] == null) return null;
        GameObject obj = resourceDictionary[resourceName].Dequeue();
        return obj;
    }

    public bool CheckQueueEmpty(string name) //�ش� �±��� ť�� ����ִ��� Ȯ���ϰ� �Ұ� ��ȯ
    {
        if (!resourceDictionary.ContainsKey(name)) return false;

        bool isIn = resourceDictionary[name].TryDequeue(out GameObject obj);
        if (!isIn) return false;
        resourceDictionary[name].Enqueue(obj);
        return isIn;
    }

    public void GetUselessResource(string name)
    {
        if (!resourceDictionary.ContainsKey(name)) return;

        
    }
}
