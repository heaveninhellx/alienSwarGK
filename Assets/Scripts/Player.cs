
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour
{
    public Image blood;
    public static Player singleton;
    Color bloodColor;
    public float maxHealth = 100f;
    public float currentHealth { get; private set; }

    void Awake()
    {
        bloodColor = blood.color;
        singleton = this;
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Color temp = bloodColor;
        bloodColor.a = 1 - currentHealth / 100;
        blood.color = temp;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        gameObject.GetComponent<FirstPersonController>().enabled = false;
        FindObjectOfType<PlayerManager>().GameOver();
    }
   
}
