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
    public Transform[] targets; // Ÿ�� ���
    public Transform currentTarget; // ���� ���õ� Ÿ��
    public float detectDistace; // ���� �Ÿ�

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

        currentTarget = nearestTarget; // ���� ����� Ÿ������ ����
    }
}
