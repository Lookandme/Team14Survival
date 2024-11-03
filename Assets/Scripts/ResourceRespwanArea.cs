using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceRespwanArea : MonoBehaviour
{
    public ResourceFactory resourceFactory;
    public ResourcePool resourcePool;

    private List<string> resourceName;

    [Header("X")]
    public float minXPoint;
    public float maxXPoint;

    [Header("Z")]
    public float minZPoint;
    public float maxZPoint;

    private bool inHuman = false; //collision 안에 사람이 있는지

    private void Awake()
    {
        resourceFactory = GetComponent<ResourceFactory>();
        resourcePool = GetComponent<ResourcePool>();
    }

    private void Start()
    {
        resourceName = resourceFactory.GetName();
    }

    private void Update()
    {
        CheckResourceSpawn();
    }

    private void CheckResourceSpawn()
    {
        if (resourceName == null) return;
        
        for (int i = 0; i < resourceName.Count; i++)
        {
            if (resourcePool.CheckQueueEmpty(resourceName[i])) //큐의 내용물을 확인하여 Spawning 호출 true 일 때 Dequeue 가능
            {
                Spawning(resourceName[i]);
            }
        }
    }
    private void Spawning(string resourceName)
    {
        GameObject obj = resourcePool.GetResourceInPool(resourceName);
        //오브젝트 생성위치에 또다른 자원이 있을 경우 다시 재설정
        obj.transform.position = ResourceSpawnPoint();

        obj.SetActive(true);
    }


    private Vector3 ResourceSpawnPoint()
    {
        Vector3 point = new Vector3(Random.Range(minXPoint,maxXPoint),0,Random.Range(minZPoint,maxZPoint));
        return transform.position + point;
    }

    private void SpawnPointCheck(Vector3 point)
    {

        Physics.Raycast(point + Vector3.up, Vector3.down, out RaycastHit inHit);
        

        
    }

  
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            inHuman = true; 
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            inHuman = false;
        }
    }
}
