using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;


[Serializable]
public class ResourceData
{
    public GameObject prefab;

    public int poolSize;
}

public class ResourceFactory : MonoBehaviour
{
    public List<ResourceData> resouceList;

    private GameObject SetFactory(string tag,ref int num)
    {
        for(int i = 0; i < resouceList.Count; i++)
        {
            if(resouceList[i].prefab.name == tag)
            {
                num = i;
                return resouceList[i].prefab;
            }      
        }
        return null;
    }

    public List<GameObject> CreateResource(string tag)
    {
        int num = -1;//팩토리 인덱스 번호
        GameObject obj = SetFactory(tag,ref num);//팩토리에서 가져온 오브젝트
        if(obj == null) return null;

        List<GameObject> list = new List<GameObject>();

        for (int i = 0; i < resouceList[num].poolSize; i++)
        {
            GameObject resource = Instantiate(obj);
            resource.SetActive(false);
            list.Add(resource);
        }

        return list;
    }

    public List<string> GetName() //팩토리에 연결된 프리팹들 이름 가져오기
    {
        if(resouceList.Count == 0) return null;

        List<string> list = new List<string>();

        for (int i = 0;i < resouceList.Count; i++)
        {
            string name = resouceList[i].prefab.name;
            list.Add(name);
        }
        return list;
    }
}