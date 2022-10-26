using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

public class EnemyController : Actor
{
    public SpriteRenderer spriteR;
    public GameObject deathPoof;
    public float blinkIntesity, blinkDuration, blinkTimer, dpoofOffset;
    public bool tookDamage, spawnedXp = false;

    public List<GameObject> drops = new List<GameObject>();
    int multiDropChance, dropIndex;

    [Header("Stats")]
    public int hpMax;
    public int str, def, spd, xp, gold;

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
        _stats.AddStat("gold",     1,5);    //How much xp to give the player
        //_stats.Fill();
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
        _stats["gold"].Value = gold;
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
            SpawnDrop();
            Instantiate(deathPoof, new Vector3(transform.position.x , transform.position.y + dpoofOffset, transform.position.z), transform.rotation);
            Destroy(this.gameObject);
        }
        return isDead;
    }

    public void SpawnDrop()
    {
        int rand1, rand2;
        List<GameObject> dropped = new List<GameObject>();
        // chance for multi drop
        multiDropChance = Random.Range(0,10);
        if(multiDropChance < 3)
        {
            rand1 = Random.Range(0,drops.Count);
            rand2 = Random.Range(0,drops.Count);
            while(rand2 == rand1)
            {
                rand2 = Random.Range(0,drops.Count);
            }

            dropped.Add(Instantiate(drops[rand1], transform.position, transform.rotation));
            dropped.Add(Instantiate(drops[rand2], transform.position, transform.rotation));
            //choose two random drops from drop list
        }
        else
        {
            dropIndex = Random.Range(0,drops.Count);
            dropped.Add(Instantiate(drops[dropIndex], transform.position, transform.rotation));
        }

        foreach(GameObject go in dropped)
        {
            if(go.tag == "XP")
            {
                go.GetComponent<MoveToPlayer>().amount = (int)_stats["xp"].Value;
            }
            else if(go.tag == "Gold")
            {
                go.GetComponent<MoveToPlayer>().amount = (int)_stats["gold"].Value;
            }
        }
    }

    public void VisualDamage()
    {
        if(blinkTimer > 0)
        {
            blinkTimer -= Time.deltaTime;
            float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
            float intensity = (lerp * blinkIntesity) + 1.0f;
            if(spriteR != null)
            {
                spriteR.material.color = Color.white * intensity;
            }
        }
        else
        {
            tookDamage = false;
        }
    }
}
