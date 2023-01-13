using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    public bool IsDead = false;
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
 

      
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    if (currentHealth < 0)
        currentHealth = 0;

        healthBar.SetHealth(currentHealth);
        
        if (currentHealth == 0)
        {
            IsDead = true;   
            anim.SetTrigger("isDead 0");
        }
    }
  

}
