using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;


public class Rifle : MonoBehaviour
{
    public FirstPersonController fps;
    private float walkSpeed;
    private float runSpeed;
    public float reloadSpeed= 4f;

    public float damage = 10f;
    public float fireRate = 15f;
    public float range = 100f;
    public float impactForce = 30f;

    public int maxAmmo = 10;
    public int currentAmmo;
    public Text ammoText;

    public float reloadTime = 1f;
    public bool isReloading = false;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    public GameObject grenadePrefab;

    public Animator animator;
    
    private float nextTimeToFire = 0f;

    void Start()
    {
        walkSpeed = fps.m_WalkSpeed;
        runSpeed = fps.m_RunSpeed;
        currentAmmo = maxAmmo; 
    }

    void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
    }
    // Update is called once per frame
    void Update()
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo.ToString();
        }
        if (isReloading && gameObject.activeSelf)
        {
            fps.m_WalkSpeed = reloadSpeed;
            fps.m_RunSpeed = reloadSpeed;
            return;
        }
       

        if (currentAmmo <= 0 || Input.GetKeyDown("r"))
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            fps.m_WalkSpeed = reloadSpeed;
            fps.m_RunSpeed = reloadSpeed;
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }       
        fps.m_WalkSpeed = walkSpeed;
        fps.m_RunSpeed = runSpeed;
        
        

    }
    IEnumerator Reload()
    {
        isReloading = true;
        animator.SetBool("Reloading", true);
        yield return new WaitForSeconds(reloadTime- .25f);
        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(.25f);
        currentAmmo = maxAmmo;
        isReloading = false;
    }
    void Shoot()
    {
        muzzleFlash.Play();
        currentAmmo--;
        if (gameObject.name == "GrenadeLauncher")
        {
            GameObject grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);
            Rigidbody rb = grenade.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * range);
            return;
        }
        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            if(hit.rigidbody !=null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);
        }
    }
}
