using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Character attributes:")]
    public int maxHealth;
    private int currentHealth;
    public int healthRegen;
    public float timeWaitRegen;
    private float timeWait;
    public float speed;
    private float currentSpeed;
    public float jumpSpeed;
    public int damage;

    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentSpeed = speed;
        InvokeRepeating("HealthRegen", 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    void HealthRegen()
    {
        if (Time.time > timeWait && currentHealth < maxHealth && currentHealth > 0)
        {
            currentHealth = Mathf.Min(currentHealth + healthRegen, maxHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth > 0)
        {
            currentHealth = Mathf.Max(0, currentHealth - damage);
            timeWait = Time.time + timeWaitRegen;
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
    public void SetCurrentHealth(int health)
    {
        currentHealth = health;
    }

    public float CurrentSpeed()
    {
        return currentSpeed;
    }

    public bool IsDead()
    {
        return isDead;
    }
}
