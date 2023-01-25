using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public HealthBar healthBar;
    public bool isDead = false;
    private Animator anim;


    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }
    }
 
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        healthBar.SetHealth(currentHealth);

        if (!isDead)
        {
            if (currentHealth == 0)
            {
                isDead = true;
                anim.SetTrigger("isDead");
                PlayerDied();
            }
        }
    }

    private void PlayerDied()
    {
        GameManager.instance.GameOver();
    }
}
