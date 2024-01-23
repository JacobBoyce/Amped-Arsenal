using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using System;

public class EnemyController : Actor
{
    public EnemyMovementController movementController;
    public GameObject spriteObj, deathPoof;
    public float blinkIntesity, blinkDuration, blinkTimer, dpoofOffset;
    public bool tookDamage, spawnedXp = false;
    private Color baseDamageColor = Color.white;
    public Color baseSpriteColor;
    private Color curDamageColor;
    private float curBlinkIntensity;

    public List<GameObject> drops = new List<GameObject>();
    int multiDropChance, dropIndex;
    public TextMeshProUGUI lifeText;

    [Space(10)]
    [Header("Effects")]
    public EffectController effectCont;

    [Space(10)]
    [Header("Base Stats")]
    public float hpMax;
    public float attk, str, def, spd, xp, gold;
    public int threatLVL;

    void Awake()
    {
        //when setting stats pull from a level or scale and use here to instantiate
        _stats = new Stats();
        _stats.AddStat("hp",   10);    // Max Health
        _stats.AddStat("attk",  1, 200);    // base attack damage to be scaled against strength
        _stats.AddStat("str",   1, 20);    // Multiply this by the damage of weapon being used. (Attk > 1)
        _stats.AddStat("def",         1);    // Multiply by damage taken. (0 > Def < 1)
        _stats.AddStat("spd",      3,50);    // Movement speed
        _stats.AddStat("luck",  10, 100);    // How lucky you are to get different upgrades or drops from enemies.
        _stats.AddStat("xp",     1, 100);    //How much xp to give the player
        _stats.AddStat("gold",   1, 100);    //How much xp to give the player
        //_stats.Fill();

        SetStats();
    }

    public void SetStats()
    {
        _stats["hp"].Max = hpMax;
        _stats["attk"].Max = attk;
        _stats["str"].Max = str;
        _stats["def"].Max = def;
        _stats["spd"].Max = spd;
        _stats["xp"].Max = xp;
        _stats["gold"].Max = gold;
        _stats.Fill();
    }

    public void IncreaseStats(float _hpScaleAmount, float _str, float _def, int waveNum, int _zoneNum)
    {
        float nHp;//, nStr, nDef;

        nHp = (_hpScaleAmount * _zoneNum) * waveNum;
        nHp = (_stats["hp"].Value * nHp) + _stats["hp"].Max;
        //nStr = _str * _zoneNum;
        //nDef = _def * _zoneNum;
        
        _stats["hp"].Max = nHp;
        _stats["hp"].Fill();
        //_stats["str"].Value = (_stats["str"].Value * nStr) + _stats["str"].Value;
        //_stats["def"].Value = (_stats["def"].Value * nDef) - _stats["def"].Value;
        _stats["spd"].IncreaseMaxBy(.1f);
        _stats["spd"].Fill();
    }

    public void IncreaseStats(float _hpScaleAmount, float _str, float _def, float timer, int _zoneNum)
    {
        
        float nHp;//, nStr, nDef;

        nHp = (_hpScaleAmount * _zoneNum) * (20 + Mathf.RoundToInt(timer % 5)/2);
        nHp = (_stats["hp"].Value * nHp) + _stats["hp"].Value;
        //nStr = _str * _zoneNum;
        //nDef = _def * _zoneNum;

        _stats["hp"].Max = nHp;
        _stats["hp"].Fill();
        //_stats["str"].Value = (_stats["str"].Value * nStr) + _stats["str"].Value;
        //_stats["def"].Value = (_stats["def"].Value * nDef) - _stats["def"].Value;
        _stats["spd"].IncreaseMaxBy(.1f);
        _stats["spd"].Fill();
    }

    public void CreateLargeEnemy(float _hpScaleAmount, float _str, float _def, int waveNum, int _zoneNum)
    {
        float nHp;//, nStr, nDef;

        nHp = ((_hpScaleAmount * _zoneNum) * waveNum) * 2;
        nHp = (_stats["hp"].Value * nHp) + _stats["hp"].Max;
        //nStr = _str * _zoneNum;
        //nDef = _def * _zoneNum;
        
        _stats["hp"].Max = nHp;
        _stats["hp"].Fill();
        //_stats["str"].Value = (_stats["str"].Value * nStr) + _stats["str"].Value;
        //_stats["def"].Value = (_stats["def"].Value * nDef) - _stats["def"].Value;
        _stats["spd"].IncreaseMaxBy(.1f);
        _stats["spd"].Fill();

        transform.localScale = new Vector3(2f,2f,2f);
    }

    public void Update()
    {
        if(tookDamage)
        {
            VisualDamage();
        }
        //lifeText.text = _stats["hp"].Value.ToString();
    }
    
    public bool HasEffect(EffectBase effect)
    {
        bool flag = false;

        if(effectCont.effectObjs.Count != 0)
        {
            foreach(GameObject eff in effectCont.effectObjs)
            {
                if(eff.GetComponent<EffectBase>().effectName.Equals(effect.effectName))
                {
                    Debug.Log(eff.GetComponent<EffectBase>().effectName + " == " + effect.effectName);
                    flag = true;
                }
            }
        }
        else
        {
            flag = false;
        }
        return flag;
    }

    public bool HasEffect(string effect)
    {
        bool flag = false;

        if(effectCont.effectObjs.Count != 0)
        {
            foreach(GameObject eff in effectCont.effectObjs)
            {
                if(eff.GetComponent<EffectBase>().effectName.Equals(effect))
                {
                    Debug.Log(eff.GetComponent<EffectBase>().effectName + " == " + effect);
                    flag = true;
                }
            }
        }
        else
        {
            flag = false;
        }
        return flag;
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

    public void TakeDamageFromEffect(float damage, Color dmgColor)
    {
        if(!AmDead())
        {
            tookDamage = true;
            curDamageColor = dmgColor;
            blinkTimer = blinkDuration;
            Set("hp", _stats["hp"].Value - Mathf.FloorToInt(damage * _stats["def"].Value));
        }
    }

    public void AddEffect(GameObject effect)
    {
        //Debug.Log("enemy telling its effect controller to add the effect");
        effectCont.AddEffect(effect, this);
    }

    public void RemoveEffect(string eName)
    {
        //Debug.Log("Enemy telling effect controller to remove the effect");
        effectCont.RemoveEffect(eName);
    }
    public void RemoveEffect(string eName, GameObject uiEffect)
    {
        //Debug.Log("Enemy telling effect controller to remove the effect");
        effectCont.RemoveEffect(eName, uiEffect);
    }

    public void UpdateEffect(GameObject effect)
    {
        effectCont.UpdateEffect(effect);
    }

    public void AttackPlayer(PlayerController player)
    {
        if(!AmDead())
        {
            player.TakeDamage(_stats["attk"].Value * _stats["attk"].Value);
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
           // chance for multi drop
        multiDropChance = Random.Range(0,10);
        if(multiDropChance < 2)
        {
            //spawn xp and gold
            SpawnDropObject(drops[0]);
            SpawnDropObject(drops[1]);
        }
        else
        {
            //spawn xp
            SpawnDropObject(drops[0]);
        }

    }

    public void SpawnDropObject(GameObject drop)
    {
        //shoot reward like it dropped it
            GameObject tempDrop;
            tempDrop = Instantiate(drop);
            tempDrop.transform.position = this.gameObject.transform.position;
            GetComponent<ShootReward>().ShootObject(GetComponent<EnemyMovementController>().visuals, tempDrop, ShootReward.ShootType.Up);

            if(tempDrop.tag == "XP")
            {
                tempDrop.GetComponent<MoveToPlayer>().amount = (int)_stats["xp"].Value;
            }
            else if(tempDrop.tag == "Gold")
            {
                tempDrop.GetComponent<MoveToPlayer>().amount = (int)_stats["gold"].Value;
            }
    }

    public void VisualDamage()
    {
        if(blinkTimer > 0)
        {
            blinkTimer -= Time.deltaTime;
            float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
            float intensity = (lerp * blinkIntesity) + 1.0f;
            if(spriteObj != null)
            {
                spriteObj.GetComponent<MeshRenderer>().material.color = Color.white * intensity;
            }
        }
        else
        {
            tookDamage = false;
            //curBlinkIntensity = blinkIntesity;
            //curDamageColor = baseDamageColor;
        }
    }
}
