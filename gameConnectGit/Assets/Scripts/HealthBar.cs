using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthBar : MonoBehaviour
{
	private Transform bar;
    private Transform damageBar;
    private TextMeshPro textHealth;
    private Color textColor;
    private Color colorBegin;
    private float disappearTime;
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        bar = transform.Find("Bar");
        damageBar = transform.Find("DamageBar");
        textHealth = transform.Find("TextHealth").GetComponent<TextMeshPro>();
        textColor = damageBar.Find("DamageBarSprite").GetComponent<SpriteRenderer>().color;
        colorBegin = textColor;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        disappearTime = 1.5f;

        
    }

    // Update is called once per frame
    void Update()
    {
        textHealth.SetText(player.CurrentHealth().ToString() + "/" + player.maxHealth.ToString());
        float healthPercent = (float)player.CurrentHealth() / (float)player.maxHealth;
        if (healthPercent < 0)
        {
            healthPercent = 0;
        }
        setSize(healthPercent);
        if (damageBar.localScale.x != bar.localScale.x)
        {
            disappearTime -= Time.deltaTime;
            if (disappearTime < 0)
            {
                float disappearSpeed = 3f;
                textColor.a -= disappearSpeed * Time.deltaTime;
                damageBar.Find("DamageBarSprite").GetComponent<SpriteRenderer>().color = textColor;
                if (textColor.a < 0)
                {
                    damageBar.localScale = new Vector3(bar.localScale.x, 1f);
                    disappearTime = 1.5f;
                    damageBar.Find("DamageBarSprite").GetComponent<SpriteRenderer>().color = colorBegin;
                    textColor.a = colorBegin.a;
                }
            }
        }
    }

    private void setSize(float sizeNormalized)
    {
        bar.localScale = new Vector3(sizeNormalized, 1f); 
    }

    private void setColor(Color color)
    {
        bar.Find("BarSprite").GetComponent<SpriteRenderer>().color = color;
    }
}
