using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float delay = 3f;
    public float radius = 5f;
    public float damage = 100f;
    public float force = 700f;

    public GameObject explosionEffect;

    float countdown;
    bool hasExploaded = false;

    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
        
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if(countdown <= 0f && !hasExploaded)
        {
            Explode();
            hasExploaded = true;
        }        
    }

    void Explode()
    {
        GameObject effect = Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(effect, 2f);
        Collider[] collidersToDestroy = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObject in collidersToDestroy)
        {     
            Player player = nearbyObject.GetComponent<Player>();
            Enemy enemy = nearbyObject.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            if(player != null)
            {
                player.TakeDamage(damage);
            }
        }
        Collider[] collidersToMove = Physics.OverlapSphere(transform.position, radius);

        foreach(Collider nearbyObject in collidersToMove)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);
            }
        }

        Destroy(gameObject);

    }
}
