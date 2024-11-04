using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

public class ResourcePool : MonoBehaviour
{
    public Dictionary<string, Queue<GameObject>> resourceDictionary = new();

    public void SetPool(string resourceName,List<GameObject> resourceList) //팩토리에서 생성한 오브젝트들을 큐에 담아 딕셔너리에 집어넣음
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

    public bool CheckQueueEmpty(string name) //해당 태그의 큐가 비어있는지 확인하고 불값 반환
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
