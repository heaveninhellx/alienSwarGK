using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float currentHealth = 100f;

    public float damage = 10f;
    public float attackSpeed = 1f;

    Animator animator;
    void Start()
    {
        GameObject child = transform.GetChild(0).gameObject;
        animator = child.GetComponent<Animator>();
    }

        

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die ()
    {
        animator.SetBool("isDead", true);
        transform.GetComponent<CapsuleCollider>().enabled = false;
        Destroy(this.gameObject, 10);
    }
}
