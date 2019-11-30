using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public int level = 1;
    public int currentExp = 0;
    private int expBase = 10;
    public float expMod = 1.3f;
    public int expLeft = 10;

    private Player player;
    // Update is called once per frame
    private void Start()
    {
        player = gameObject.GetComponent<Player>();
    }
    void Update()
    {
    }

    public void GainExp(int exp)
    {
        currentExp += exp;
        if (currentExp >= expLeft)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentExp -= expLeft;
        level++;
        float t = Mathf.Pow(expMod, level);
        expLeft = (int)Mathf.Floor(expBase * t);

        player.maxHealth += (int)(player.maxHealth * 0.1f);
        player.SetCurrentHealth(player.maxHealth);
        player.damage += 10;
    }
}
