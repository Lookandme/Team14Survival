
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
    Idle,
    Wandering,
    Attacking,
    Fleeing
}

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public int health;
    public float walkSpeed;
    public float runSpeed;

    [Header("AI")]
    private AIState aiState;
    public float detectDistace;
    public float safeDistace;

    [Header("Wandering")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;
    public float maxWanderWaitTime;

    [Header("Combat")]
    public int damage;
    public float attackRate;
    private float lastAttackTime;
    public float attackDistance;

    private float playerDistance;

    

  


    private NavMeshAgent agent;
    private Animator animator;
    private SkinnedMeshRenderer[] skinnedMeshes;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        skinnedMeshes = GetComponentsInChildren<SkinnedMeshRenderer>();
    }
    private void Start()
    {
        SetState(AIState.Wandering);
    }
    private void Update()
    {
        playerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player. transform.position);
        Debug.Log(playerDistance);
        animator.SetBool("Moving", aiState != AIState.Idle);
        




        switch (aiState)
        {
            case AIState.Idle:
                PassiveUpdate();
                break;
            case AIState.Wandering:
                PassiveUpdate();
                break;
            case AIState.Attacking:
                AttackingUpdate();
                break;
            case AIState.Fleeing:
                FleeingUpdate();
                break;
        }
    }


    private void SetState(AIState state)
    {
        aiState = state;

        switch (aiState)
        {
            case AIState.Idle:
                agent.speed = walkSpeed;
                agent.isStopped = true;
                break;
            case AIState.Wandering:
                agent.speed = walkSpeed;
                agent.isStopped = false;
                break;
            case AIState.Attacking:
                agent.speed = runSpeed;
                agent.isStopped = false;
                break;
            case AIState.Fleeing:
                agent.speed = runSpeed;
                agent.isStopped = false;
                break;
        }

        animator.speed = agent.speed / walkSpeed;
    }

    //private void UpdateTurningAnimation()    // ���� ��ȯ ������ �� �� ��Ȯ�ϰ� �� �ʿ䰡 ���� ���� ����
    //{
    //    float currentPositionX = transform.position.x;

    //    if (currentPositionX > previousPositionX)
    //    {
    //        // ���������� �̵� ��
    //        animator.SetBool("TurningRight", true);
           
    //    }
    //    else if (currentPositionX < previousPositionX)
    //    {
    //        // �������� �̵� ��
    //        animator.SetBool("TurningLeft", true);
           
    //    }
    //    else
    //    {
    //        // �������� ���� ���� ���� ��� false�� ����
    //        animator.SetBool("TurningRight", false);
    //        animator.SetBool("TurningLeft", false);
    //    }

    //    // ���� ��ġ ������Ʈ
    //    previousPositionX = currentPositionX;
    //}

    private void PassiveUpdate()
    {
        if (aiState == AIState.Wandering && agent.remainingDistance < 0.1f)
        {
            SetState(AIState.Idle);
            Invoke("WanderingLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));

        }
        if (playerDistance < detectDistace)
        {
            SetState(AIState.Attacking);
        }

    }

    void AttackingUpdate()
    {
        if (playerDistance > attackDistance)
        {
            agent.isStopped = false;
            NavMeshPath path = new NavMeshPath();
            if (agent.CalculatePath(CharacterManager.Instance.Player.transform.position, path))
            {
                agent.SetDestination(CharacterManager.Instance.Player.transform.position);
            }
            else
            {
                SetState(AIState.Fleeing);
            }
        }
        else
        {
            agent.isStopped = true;
            if (Time.time - lastAttackTime > attackRate)
            {
                lastAttackTime = Time.time;
               // CharacterManager.Instance.Player.controller.GetComponent<IDamagable>().TakePhysicalDamage(damage);  ������ ������ �κ�
                animator.speed = 1;
                animator.SetTrigger("Attack");
            }
        }
    }
    private void FleeingUpdate()
    {

    }

    private void WanderingLocation()
    {
        if (aiState != AIState.Idle) return;

        SetState(AIState.Wandering);
        if (playerDistance > detectDistace)
        {
            agent.SetDestination(GetWanderLocation());
        }
        
      
    }
    Vector3 GetWanderLocation()  // ��� �������� ���� �ִ� ���� ������ �������� �������ִ� �ڵ�
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance,NavMesh.AllAreas);

        int i = 0;
        while (Vector3.Distance(transform.position, hit.position) < detectDistace)  // do while ������ �ٲ㺸��
        {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30)
                break;
        }

        return hit.position;
    }

    public void TakePhysicalDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
            Die();


       
    }

    private void Die()
    {
       Destroy(gameObject);
    }
}
