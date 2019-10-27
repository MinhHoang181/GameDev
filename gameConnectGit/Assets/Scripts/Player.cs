using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Character attributes:")]
    public int maxHealth;
    private int currentHealth;
    public float speed;
    private float currentSpeed;
    public float jumpSpeed;
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentSpeed = speed;
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

    public void EffectSlow(float slowPercent, float timeSlow)
    {
        currentSpeed *= slowPercent;
        StartCoroutine(SpeedNormal(timeSlow));
    }

    IEnumerator SpeedNormal(float time)
    {
        yield return new WaitForSeconds(time);
        currentSpeed = speed;
    }

    public int CurrentHealth()
    {
        return currentHealth;
    }

    public float CurrentSpeed()
    {
        return currentSpeed;
    }
}
