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
        Queue<GameObject> resourceQueue = new Queue<GameObject>(); //리스트의 내용을 넣을 큐 생성
        for(int i = 0; i< resourceList.Count; i++)
        {
            resourceQueue.Enqueue(resourceList[i]); //리스트의 오브젝트를 큐에 Enqueue
        }
        resourceDictionary.Add(resourceName, resourceQueue);    //딕셔너리에 해당 프리팹의 이름과 오브젝트를 담은 큐를 등록
    }

    public GameObject GetResourceInPool(string resourceName)
    {
        if (!resourceDictionary.ContainsKey(resourceName)) return null; //해당 이름의 키가 없다면 돌아감

        if (resourceDictionary[resourceName] == null) return null; 
        GameObject obj = resourceDictionary[resourceName].Dequeue();
        return obj;
    }

    public bool CheckQueueEmpty(string name) 
    {
        if (!resourceDictionary.ContainsKey(name)) return false; // 키가 없을 경우 false

        bool isIn = resourceDictionary[name].TryDequeue(out GameObject obj); //TryDequeue로 Dequeue가 가능한지 확인한다.
        if (!isIn) return false;  //Dequeue가 불가능 false
        resourceDictionary[name].Enqueue(obj);//Dequeue가 되었을 땐 다시 꺼낸 오브젝트를 집어넣어준다.
        return isIn; //true를 반환
    }

    public void GetUselessResource()
    {
        Resource[] objects = GetComponentsInChildren<Resource>(true); //오브젝트의 하위 게임오브젝트들을 가져옴 true를 쓴 이유는 비활성화 된 오브젝트를 찾기 위해
        foreach (Resource obj in objects)
        {
            if(obj.GetComponent<Resource>().capacity == 0 && !obj.gameObject.activeSelf) //해당 오브젝트의 리소스 컴포넌트 정보를 가져와 체력이 0이고 비활성화 된 것을 찾음
            {
                obj.GetComponent<Resource>().capacity = 10; //오브젝트의 체력을 원상 복구

                string name = obj.name;
                name = name.Replace("(Clone)","");//키를 입력하기 위해 복제된 프리팹의 이름을 가져와 clone을 제거
                Debug.Log(name);

                resourceDictionary[name].Enqueue(obj.gameObject) ; //해당오브젝트의 태그에 맞는 키 값에 입력
            }
        }
    }
}
