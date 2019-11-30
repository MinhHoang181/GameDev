using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    public new string name;
    [Header("Enemies stats:")]
    public int level = 1;
    public int maxHealth;
    private int currentHealth;
    public int damage;
    public float speed = 1f;
    [Space]
    [Header("Exp:")]
    public int exp;
    [Header("Point:")]
    public int point;

    private bool isDamaged;
    private bool isDead = false;

    private TextMeshPro textName;
    private TextMeshPro textLevel;

    public Transform DamagePopup;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth += (int)(maxHealth * 0.5f) * (level - 1);
        damage += (int)(damage * 0.3f) * (level - 1);
        exp += level - 1;
        point += point * (level - 1);
        currentHealth = maxHealth;
        textName = transform.Find("Name").GetComponent<TextMeshPro>();
        textName.SetText(name);
        textLevel = transform.Find("Level").GetComponent<TextMeshPro>();
        textLevel.SetText("Lv " + level.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0 && !isDead)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<LevelSystem>().GainExp(exp);
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().SetScore(point);
            isDead = true;
        }
    }

    public void TakeDamage(int damage)
    {
        
        if (currentHealth > 0)
        {
            // Hien thi so damage
            Transform damgagePopupTransform = Instantiate(DamagePopup, transform.position, Quaternion.identity);
            DamagePopup damagePopup = damgagePopupTransform.GetComponent<DamagePopup>();
            damagePopup.Setup(damage);

            currentHealth = Mathf.Max(0, currentHealth - damage);
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
