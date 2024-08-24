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
    public bool tookDamage, spawnedXp = false, isLargeEnemy = false, triggeredDeath = false;
    [Range(1,10),Header("1 = 10%  10 = 100%")]
    public int largeEnemyRelicDropChance;
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

    [Range(0.1f, 0.5f)]
    public float pitchMultiplier;

    void Awake()
    {
        //when setting stats pull from a level or scale and use here to instantiate
        _stats = new Stats();
        _stats.AddStat("hp",   10);    // Max Health
        _stats.AddStat("attk",  1, 200);    // base attack damage to be scaled against strength
        _stats.AddStat("str",   1, 20);    // Multiply this by the damage of weapon being used. (Attk > 1)
        _stats.AddStat("def",         1);    // Multiply by damage taken. (0 > Def < 1)
        _stats.AddStat("spd",      3,15);    // Movement speed
        _stats.AddStat("luck",  10, 100);    // How lucky you are to get different upgrades or drops from enemies.
        _stats.AddStat("xp",     1, 100);    //How much xp to give the player
        _stats.AddStat("gold",   1, 100);    //How much xp to give the player
        //_stats.Fill();

        SetStats();
    }

    public void SetStats()
    {
        _stats["hp"].Value = hpMax;
        _stats["attk"].Value = attk;
        _stats["str"].Value = str;
        _stats["def"].Value = def;
        _stats["spd"].Value = spd;
        _stats["xp"].Value = xp;
        _stats["gold"].Value = gold;
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
        _stats["spd"].IncreaseByPercent(.1f);
        //_stats["spd"].Fill();
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
        _stats["spd"].IncreaseByPercent(.1f);
        //_stats["spd"].Fill();
    }

    public void CreateLargeEnemy(float _hpScaleAmount, float _str, float _def, int waveNum, int _zoneNum)
    {
        isLargeEnemy = true;
        float nHp;//, nStr, nDef;

        nHp = ((_hpScaleAmount * _zoneNum) * waveNum) * 2;
        nHp = (_stats["hp"].Value * nHp) + _stats["hp"].Max;
        //nStr = _str * _zoneNum;
        //nDef = _def * _zoneNum;
        
        _stats["hp"].Max = nHp;
        //_stats["hp"].Fill();
        //_stats["str"].Value = (_stats["str"].Value * nStr) + _stats["str"].Value;
        //_stats["def"].Value = (_stats["def"].Value * nDef) - _stats["def"].Value;
        _stats["spd"].IncreaseByPercent(.1f);
        //_stats["spd"].Fill();

        transform.localScale = new Vector3(2f,2f,2f);
    }

    public void Update()
    {
        if(tookDamage)
        {
            VisualDamage();
        }
        //lifeText.text = _stats["hp"].Value.ToString();
        lifeText.text = movementController.enemyState.ToString();
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
                    //Debug.Log(eff.GetComponent<EffectBase>().effectName + " == " + effect.effectName);
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
                    //Debug.Log(eff.GetComponent<EffectBase>().effectName + " == " + effect);
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
        //damagedSound.PlayOneShot(damagedSound.clip);
        tookDamage = true;
        blinkTimer = blinkDuration;
        Debug.Log(Mathf.FloorToInt(damage * _stats["def"].Value));
        Set("hp", _stats["hp"].Value - Mathf.FloorToInt(damage * _stats["def"].Value));

        if(AmDead())
        {
            //call death stuff
            if(triggeredDeath == false)
            {
                DoDeathStuff();
            }
        }
    }

    public void TakeDamageFromEffect(float damage, Color dmgColor)
    {
        tookDamage = true;
        //curDamageColor = dmgColor;
        blinkTimer = blinkDuration;
        Set("hp", _stats["hp"].Value - Mathf.FloorToInt(damage * _stats["def"].Value));

        if(AmDead())
        {
            //call death stuff
            if(triggeredDeath == false)
            {
                DoDeathStuff();
            }
        }
    }

    public void DoDeathStuff()
    {
        triggeredDeath = true;
        
        if(spawnedXp == false)
        {
            if(isLargeEnemy)
            {
                //make death poof bigger
                GameObject largeDPoof = ObjectPoolManager.SpawnObject(deathPoof, new Vector3(transform.position.x , transform.position.y + dpoofOffset, transform.position.z), transform.rotation, ObjectPoolManager.PoolType.DPoof);
                largeDPoof.transform.localScale += new Vector3(largeDPoof.transform.localScale.x *1.5f, largeDPoof.transform.localScale.y *1.5f, largeDPoof.transform.localScale.z *1.5f);
                largeDPoof.GetComponent<DeathPoofLogic>().isLarge = true;

                //chance to drop relic
                if(Random.Range(0, 10) < largeEnemyRelicDropChance)
                {
                    GameObject relicToSpawnObj = Instantiate(GameZoneController.Instance.relicLibrary.relicList[Random.Range(0,GameZoneController.Instance.relicLibrary.relicList.Count)], GetComponent<EnemyMovementController>().visuals.transform.position, Quaternion.identity);
                    GetComponent<EnemyMovementController>().visuals.GetComponent<ShootReward>().ShootObject(GetComponent<EnemyMovementController>().visuals, relicToSpawnObj, ShootReward.ShootType.Facing);
                
                }
                else
                {
                    _stats["xp"].Value = 10;
                    _stats["gold"].Value = 10;
                    spawnedXp = true;
                    SpawnDrop();
                }
            }
            else
            {
                spawnedXp = true;
                SpawnDrop();
                ObjectPoolManager.SpawnObject(deathPoof, new Vector3(transform.position.x , transform.position.y + dpoofOffset, transform.position.z), transform.rotation, ObjectPoolManager.PoolType.DPoof);
            }
            spriteObj.SetActive(false);
            effectCont.uiSatusEffectParent.SetActive(false);
            Destroy(this.gameObject, .5f);
            //StartCoroutine(ReturnToPoolAfterTime());
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
        return _stats["hp"].Value <= 0;        
    }

    private IEnumerator ReturnToPoolAfterTime()
    {
        yield return new WaitForSeconds(0.5f);
        ObjectPoolManager.ReturnObjectToPool(this.gameObject);
    }

    public void SpawnDrop()
    {
           // chance for multi drop
        multiDropChance = Random.Range(0,10);
        if(multiDropChance < 2)
        {
            //spawn xp and gold
            GameObject tempGoldDrop = ObjectPoolManager.SpawnObject(drops[1], transform.position, transform.rotation, ObjectPoolManager.PoolType.GoldNug);
            tempGoldDrop.GetComponent<MoveToPlayer>().amount = (int)_stats["gold"].Value;
            GetComponent<ShootReward>().ShootObject(GetComponent<EnemyMovementController>().visuals, tempGoldDrop, ShootReward.ShootType.Up);
        }
            //spawn xp
        GameObject tempDrop = ObjectPoolManager.SpawnObject(drops[0], transform.position, transform.rotation, ObjectPoolManager.PoolType.XpOrbParent);
        tempDrop.GetComponent<MoveToPlayer>().amount = (int)_stats["xp"].Value;
        GetComponent<ShootReward>().ShootObject(GetComponent<EnemyMovementController>().visuals, tempDrop, ShootReward.ShootType.Up);
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
