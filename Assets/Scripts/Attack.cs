using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class Attack : MonoBehaviour
{
    Enemy enemy;
    public float attackCd = 0f;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
    }
    void Update()
    {
        attackCd -= Time.deltaTime;
    }

    // Update is called once per frame
    public void Combat(Player player)
    {
        if (attackCd <= 0)
        {
            player.TakeDamage(enemy.damage);
            attackCd = 1f / enemy.attackSpeed;
        }
        
    }
}
