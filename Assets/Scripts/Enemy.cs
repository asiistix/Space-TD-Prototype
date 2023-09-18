using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float MaxHealth = 100;
  public  float currentHealth;

    void Start()
    {
        currentHealth = MaxHealth;
    }

   public void TakeDamage(float Damage)
    {
        currentHealth -= Damage;
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        gameObject.SetActive(false);
    }
}
