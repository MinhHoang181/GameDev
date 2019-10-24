using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Character attributes:")]
    public int maxHealth;
    private int currentHealth;
    public float speed;
    public float jumpSpeed;
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
        }
    }

    public int CurrentHealth()
    {
        return currentHealth;
    }
}
