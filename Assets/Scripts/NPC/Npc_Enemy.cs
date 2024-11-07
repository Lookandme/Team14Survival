using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc_Enemy : MonoBehaviour
{
    [Header("Default")]
    public int npcHealth;
    public float walkSpeed;
    public float runSpeed;


    [Header("AI")]
    public Transform[] targets; // 타겟 목록
    public Transform currentTarget; // 현재 선택된 타겟
    public float detectDistace; // 감지 거리

    [Header("Wandering")]
    public float minWanderDistance;
    public float maxWanderDistance;

   // [Header("")]



    private void FindNearestTarget()
    {
        Transform nearestTarget = null;
        float nearestDistance = detectDistace;

        foreach (Transform target in targets)
        {
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestTarget = target;
            }
        }

        currentTarget = nearestTarget; // 가장 가까운 타겟으로 설정
    }
}
