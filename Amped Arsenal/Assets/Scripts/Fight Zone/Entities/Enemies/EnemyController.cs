using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyController : Actor
{
    public SpriteRenderer spriteR;
    public GameObject xpPrefab;
    public float blinkIntesity, blinkDuration, blinkTimer;
    public bool tookDamage, spawnedXp = false;

    [Header("Stats")]
    public int hpMax;
    public int str, def, spd, xp;

    void Awake()
    {
        //when setting stats pull from a level or scale and use here to instantiate
        _stats = new Stats();
        _stats.AddStat("hp",        10);    // Max Health
        _stats.AddStat("str",      1,2);    // Multiply this by the damage of weapon being used. (Attk > 1)
        _stats.AddStat("def",        1);    // Multiply by damage taken. (0 > Def < 1)
        _stats.AddStat("spd",     3,50);    // Movement speed
        _stats.AddStat("luck",      10);    // How lucky you are to get different upgrades or drops from enemies.
        _stats.AddStat("xp",       1,5);    //How much xp to give the player
        //_stats.Fill();
        xpPrefab.GetComponent<MoveToPlayer>().xpAmount = (int)_stats["xp"].Value;
        spriteR = GetComponentInChildren<SpriteRenderer>();

        SetStats();
    }

    public void SetStats()
    {
        _stats["hp"].Value = hpMax;
        _stats["str"].Value = str;
        _stats["def"].Value = def;
        _stats["spd"].Value = spd;
        _stats["xp"].Value = xp;
    }

    public void Update()
    {
        if(tookDamage)
        {
            VisualDamage();
        }
    }
    

    public void TakeDamage(float damage)
    {
        if(!AmDead())
        {
            tookDamage = true;
            blinkTimer = blinkDuration;
            Set("hp", _stats["hp"].Value - Mathf.FloorToInt(damage * _stats["def"].Value));
        }
    }

    public void AttackPlayer(PlayerController player)
    {
        if(!AmDead())
        {
            player.TakeDamage(_stats["str"].Value/* multiply by level of enemy*/);
        }
    }

    public bool AmDead()
    {
        bool isDead = false;
        isDead = _stats["hp"].Value <= 0 ? true : false;
        if(isDead && spawnedXp == false)
        {
            spawnedXp = true;
            Instantiate(xpPrefab, transform.position, transform.rotation);
        }
        return isDead;
    }

    public void VisualDamage()
    {
        if(blinkTimer > 0)
        {
            blinkTimer -= Time.deltaTime;
            float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
            float intensity = (lerp * blinkIntesity) + 1.0f;
            spriteR.material.color = Color.white * intensity;
        }
        else
        {
            tookDamage = false;
        }
    }
}
