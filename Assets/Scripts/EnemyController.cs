using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float lookRadius = 10f;
    public float damage = 20f;
    public bool canAttack = true;
    Transform target;
    NavMeshAgent agent;
    Attack attack;
    Animator animator;
    private Enemy enemy;
    [SerializeField]


    void Start()
    {
        enemy = transform.GetComponent<Enemy>();
        GameObject child = transform.GetChild(0).gameObject;
        animator = child.GetComponent<Animator>();


        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        attack = GetComponent<Attack>();
    }
    void Update()
    {
        if (enemy.currentHealth <= 0)
            return;
        if(Player.singleton.currentHealth <= 0)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", false);
            return;
        }

        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= lookRadius)
        {
            if (distance <= agent.stoppingDistance && canAttack)
            {
                AttackPlayer();
                return;
            }
            if(distance > agent.stoppingDistance)
            ChasePlayer();
            FaceTarget();

        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
    void ChasePlayer()
    {
        agent.SetDestination(target.position);
        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);
    }
    void AttackPlayer()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", true);
        StartCoroutine(AttackTime());
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Zombie Neck Bite"))
            StopCoroutine(AttackTime());
    }
    IEnumerator AttackTime()
    {     
        canAttack = false;   
        yield return new WaitForSeconds(1.1f);
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Zombie Neck Bite"))
        {
            canAttack = true;
            yield break;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Zombie Neck Bite"))
        {
            Player.singleton.TakeDamage(damage);
        }           
        yield return new WaitForSeconds(1.1f);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Zombie Neck Bite"))
        {
            Player.singleton.TakeDamage(damage);
        }
        canAttack = true;

    }

}
