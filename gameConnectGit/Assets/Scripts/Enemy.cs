using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemies stats:")]
    public int maxHealth;
    private int currentHealth;

    private bool isDamaged;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //isDamaged = false;
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            isDamaged = true;
        }
    }

    public int CurrentHealth()
    {
        return currentHealth;
    }

    public bool IsDamaged()
    {
        return isDamaged;
    }

    public void IsDamaged(bool set)
    {
        isDamaged = set;
    }
}
