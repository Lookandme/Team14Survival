
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
    Idle,
    Wandering,
    Attacking,
    Chasing,

}

public class Enemy : MonoBehaviour, IDamagable
{
    [Header("Stats")]
    public float health;
    public float walkSpeed;
    public float runSpeed;

    [Header("AI")]
    private AIState aiState;
    public float detectDistace;
   

    [Header("Wandering")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;
    public float maxWanderWaitTime;

    [Header("Combat")]
    public float damage;
    public float attackRate;
    private float lastAttackTime = 0f;
    public float attackDistance;

    private float playerDistance;
    private bool isAttacking = false; 

    private NavMeshAgent agent;
    private Animator animator;
    private SkinnedMeshRenderer[] skinnedMeshes;
    public LayerMask targetMask;
    PlayerConditions playerConditions;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        skinnedMeshes = GetComponentsInChildren<SkinnedMeshRenderer>();
    }
    private void Start()
    {

        StartCoroutine(nameof(StartEuemy));


    }
    private void Update()
    {
        playerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position);
        Debug.Log(playerDistance);
        switch (aiState)
        {
            case AIState.Idle:
                StartCoroutine(nameof(StartEuemy));
                break;
            case AIState.Wandering:
                WanderingUpdate();
               
                break;
            case AIState.Attacking:
                AttackingUpdate();
                break;
            case AIState.Chasing:
                ChasingUpdate();
                
                break;



        }
        ChangedAnimation();

    }

    private IEnumerator StartEuemy()  // ó�� �ֳʹ� ��ȯ ��� �� ������ ����
    {
        if(health != 0)
        {
            
            SetState(AIState.Idle);
           
            yield return new WaitForSeconds(6f);
            SetState(AIState.Wandering);
            Debug.Log("@@@");
        }
        else
        {
            animator.SetBool("Die",true);
            
            
        }
    }
    private void ChangedAnimation()
    {
        if(aiState == AIState.Wandering)
        {
            animator.SetBool("Moving", true);
        }
        else animator.SetBool("Moving", false);

        if (aiState != AIState.Chasing)
        {
            animator.SetBool("Chasing", true);
        } 
        else animator.SetBool("Chasing", false);

        //if (aiState != AIState.Attacking)
        //{
        //    
        //    agent.isStopped = false;
        //}
        //else
        //{
        //    
        //    agent.isStopped = true;
        //}
    }


    private void SetState(AIState state)
    {
        aiState = state;

        switch (aiState)
        {
            case AIState.Idle:
                break;
            case AIState.Wandering:
                agent.speed = walkSpeed;
                break;
            case AIState.Attacking:
                agent.speed = walkSpeed;
                break;
            case AIState.Chasing:
                agent.speed = runSpeed;
                break;


        }

        animator.speed = agent.speed / walkSpeed;
    }



    private void WanderingUpdate()
    {
        if (playerDistance > detectDistace && agent.remainingDistance <= 0.1f)
        {
            WanderingLocation();

        }
        if (playerDistance < detectDistace)
        {
            SetState(AIState.Chasing);

        }

    }
    private void ChasingUpdate() // ��ǥ���� ���󰡴� ���� ��ǥ ���̰Ÿ��� ���� ���� ����
    {
        animator.ResetTrigger("Attack");
        agent.SetDestination(CharacterManager.Instance.Player.transform.position + Vector3.up);
        Debug.Log($"destination {agent.destination}");

        if (playerDistance > detectDistace)
        {
            SetState(AIState.Wandering);
        }

        if (playerDistance < attackDistance)
        {
            SetState(AIState.Attacking);
            agent.ResetPath();
            
        }
    }


    private void AttackingUpdate() // ������ ���ݿ� �����̰� ���»��� ���� �ʿ� !!
    {
       
        if (Time.time - lastAttackTime >= attackRate)   // ������ ������ �� �ִ��� üũ
        {
            agent.ResetPath();
            animator.SetTrigger("Attack");

            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, attackDistance, targetMask))
            {
                hit.collider.GetComponent<IDamagable>().GetDamage(damage);
                
            }

            
            lastAttackTime = Time.time;  // ������ ���� �ð� ������Ʈ
        }
        if(playerDistance > attackDistance) // ���� ������ ����ٸ�?
        {
            SetState(AIState.Chasing);
        }
    }
    private void WanderingLocation()
    {
        if (playerDistance > detectDistace) // �÷��̾� ��ġ�� ���� ���� �ۿ� �ִٸ�
        {
            agent.SetDestination(GetWanderLocation());
        }


    }
    Vector3 GetWanderLocation()  // ��� ���������� ���� �ִ� ���� ������ �������� �������ִ� �ڵ�
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);

        int i = 0;
        while (Vector3.Distance(transform.position, hit.position) < detectDistace)  // do while ������ �ٲ㺸�� ��������...
        {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30)
                break;
        }

        return hit.position;
    }

    public void GetDamage(float damage) // �÷��̾� ���� �������� �ִ� �ż���
    {
        animator.SetTrigger("GetDamage");
        health -= damage;
        if (health <= 0)
            Die();

    }


    


    private void Die()
    {
        SetState(AIState.Idle);

        

       GameObject.Destroy(gameObject);

    }
}
