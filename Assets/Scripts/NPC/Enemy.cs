
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
    public float attackDistance;

    private float playerDistance;
     

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
        agent.stoppingDistance = attackDistance;

    }
    private void Update()
    {
        playerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position);
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
            yield return new WaitForSeconds(6f);
            SetState(AIState.Wandering);
    }
    private void ChangedAnimation()
    {
        if(aiState == AIState.Wandering)
        {
            animator.SetBool("Moving", true);
        }
        else animator.SetBool("Moving", false);

        if (aiState == AIState.Chasing)
        {
            animator.SetBool("Chasing", true);
        } 
        else animator.SetBool("Chasing", false);

        
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
        
        if (playerDistance > detectDistace && agent.remainingDistance <= agent.stoppingDistance)
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
        
        agent.SetDestination(CharacterManager.Instance.Player.transform.position + Vector3.up);
        

        if (agent.remainingDistance > detectDistace)
        {
            SetState(AIState.Wandering);
        }

        if (agent.remainingDistance <= attackDistance)
        {
            SetState(AIState.Attacking);
           
            
        }
    }


    private void AttackingUpdate() // ������ �ذ� �ڷ�ƾ���� �ۼ�
    {
        agent.velocity = Vector3.zero; // ������ ���� �Ǹ� ������Ʈ�� ������Ѵ�
        StartCoroutine(nameof(Attacking));
        if (playerDistance > detectDistace)
        {
            StopCoroutine(nameof(Attacking));
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

    private IEnumerator Attacking()  // �÷��̾ ���� ������ ������ ���� ���ݸ��� ���� �ִϸ��̼� 
    {
        while (playerDistance < attackDistance)
        {
            animator.SetTrigger("Attack");

            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, attackDistance, targetMask))
            {
                hit.collider.GetComponent<IDamagable>().GetDamage(damage);
            }
            yield return new WaitForSeconds(attackRate);
        }


    }

    public void GetDamage(float damage) // �÷��̾� ���� �������� �ִ� �ż��� ü���� �ٴڳ��� ���� ���°� �ȴ�
    {
        animator.SetTrigger("GetDamage");
        health -= damage;
        if (health <= 0)
            StartCoroutine(nameof(Die));
        
    }
    private IEnumerator Die() // ���� �ִϸ��̼� ����� 5�� �� ���� ������Ʈ ����
    {
        animator.SetBool("Die", true);
        yield return new WaitForSeconds(5f);
        GameObject.Destroy(gameObject);
    }





}
