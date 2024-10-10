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
    public bool tookDamage, spawnedXp = false, isLargeEnemy = false, triggeredDeath = false, inExfilPhase;


    public List<GameObject> drops = new();
    int goldDropChance, xpDropChance, baseXPDropChance;
    public TextMeshProUGUI lifeText;

    [Space(10)]
    [Header("Effects")]
    public EffectController effectCont;

    [Space(10)]
    [Header("Base Stats")]
    public float hpMax;
    public float attk, str, def, spd, xp, gold;



    void Awake()
    {
        //when setting stats pull from a level or scale and use here to instantiate
        _stats = new Stats();
        _stats.AddStat("hp",   0);    // Max Health
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
        _stats["hp"].Max = hpMax;
        _stats["attk"].Value = attk;
        _stats["str"].Value = str;
        _stats["def"].Value = def;
        _stats["spd"].Value = spd;
        _stats["xp"].Value = xp;
        _stats["gold"].Value = gold;
    }

    public void IncreaseStats(float _waveScale, float _levelScale, float _str, float _def, int waveNum, int _zoneNum)
    {
        baseXPDropChance = 90;
        float nHP = _stats["hp"].Max * (1 + (waveNum * _waveScale) + (_zoneNum - 1) * _levelScale);
 
        _stats["hp"].Max = nHP;
        _stats["hp"].Fill();

        _stats["str"].Value += .01f;
        if(_stats["def"].Value > .75)
        {
            _stats["def"].Value -= _def;
        }

        if(_zoneNum > 1)
        {
            _stats["spd"].IncreaseByAmount(_zoneNum+2);
        }
        _stats["spd"].IncreaseByPercent(.1f);
    }

    public void IncreaseStats(float _waveScale, float _levelScale, float _interval, float _str, float _def, float timer, int waveNum, int _zoneNum)
    {    
        waveNum += Mathf.RoundToInt(timer / _interval);


        baseXPDropChance = 30;

        baseXPDropChance = Mathf.RoundToInt(baseXPDropChance - (waveNum - 15) * .5f);
        if(baseXPDropChance < 1)
        {
            baseXPDropChance = 1;
        }

        float nHP = _stats["hp"].Max * (1 + (waveNum * _waveScale) + (_zoneNum - 1) * _levelScale);
        _stats["hp"].Max = nHP;
        _stats["hp"].Fill();

        float newDef = _stats["def"].Value - (waveNum - 15) * _def;
        if(newDef > .25f)
        {
            _stats["def"].Value = newDef;
        }
        else
        {
            _stats["def"].Value = .25f;
        }

        float newStr = _stats["str"].Value + (waveNum - 15) * _str;
        _stats["str"].Value = newStr;
        Debug.Log("normal enmy str: " + newStr);

        //float newSpd = _stats["spd"].Value + (waveNum - 15) * .05f;
        _stats["spd"].Value = _stats["spd"].Value * (1 + .01f * waveNum);
        //Debug.Log(_stats["spd"].Value);
        //Debug.Log("spd to add " + newEnemySpd);

        //_stats["spd"].IncreaseByAmount(newEnemySpd);
        //_stats["spd"].IncreaseByPercent(.2f);
    }

    public void CreateLargeEnemy(float _waveScale, float _levelScale, float _exfilScale, float timer, float _interval, float _str, float _def, int waveNum, int _zoneNum, bool isExfil)
    {
        inExfilPhase = isExfil;
        waveNum += Mathf.RoundToInt(timer / _interval);

        float nHP;


        if(isExfil)
        {
            
            nHP = 10 * (_stats["hp"].Max * (1 + (waveNum * _waveScale) + (_zoneNum - 1) * _levelScale));
            Debug.Log(nHP);
        }
        else
        {
            nHP = 5 * (_stats["hp"].Max * (1 + (waveNum * _waveScale) + (_zoneNum - 1) * _levelScale));
        }
        
        
        _stats["hp"].Max = nHP;
        _stats["hp"].Fill();
        _stats["def"].Value = .75f;
        
        if(isExfil)
        {
            float newDef = _stats["def"].Value - (waveNum - 15) * _def;

            if(newDef > .25f)
            {
                _stats["def"].Value = newDef;
            }
            else
            {
                _stats["def"].Value = .25f;
            }

            float newStr = 1 + _stats["str"].Value + (waveNum - 15) * _str;
            _stats["str"].Value = newStr;


            _stats["spd"].Value = _stats["spd"].Value * (1 + .01f * waveNum);
            //Debug.Log(_stats["spd"].Value);
            //_stats["spd"].IncreaseByAmount(newEnemySpd);
            //_stats["spd"].IncreaseByPercent(.2f);
        }

        //_stats["spd"].IncreaseByPercent(.2f);
        //_stats["spd"].Fill();i

        isLargeEnemy = true;

        transform.localScale = new Vector3(2f,2f,2f);
        if(GetComponentInChildren<VisualEffects>().wantBob == true)
        {
            GetComponentInChildren<VisualEffects>().offset += 1;
        }
    }

    public void Update()
    {
        if(tookDamage)
        {
            VisualDamage();
        }
        lifeText.text = _stats["hp"].Value.ToString();
        //lifeText.text = movementController.enemyState.ToString();
    }

    public void ToggleViewHP(bool toggle)
    {
        lifeText.gameObject.SetActive(toggle);
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
        //Debug.Log(Mathf.FloorToInt(damage * _stats["def"].Value));
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

    public void TakeDamageFromEffect(float damage)
    {
        tookDamage = true;
        //curDamageColor = dmgColor;
        blinkTimer = blinkDuration;
        Set("hp", _stats["hp"].Value - damage);

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
                spawnedXp = true;
                SpawnLargeDrop();
            }
            else
            {
                spawnedXp = true;
                SpawnDrop();
                ObjectPoolManager.SpawnObject(deathPoof, new Vector3(transform.position.x , transform.position.y + dpoofOffset, transform.position.z), transform.rotation, ObjectPoolManager.PoolType.DPoof);
            }
            spriteObj.SetActive(false);
            effectCont.uiSatusEffectParent.SetActive(false);
            effectCont.effectSpwnPointOnEnemy.SetActive(false);
            Destroy(this.gameObject, 1f);
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
            player.TakeDamage(Mathf.CeilToInt(_stats["attk"].Value * _stats["str"].Value));
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
        goldDropChance = Random.Range(0,101);
        if(goldDropChance < PlayerPrefs.GetInt("Inflation")) //inflation should be a whole number between 0-50
        {
            //spawn xp and gold
            GameObject tempGoldDrop = ObjectPoolManager.SpawnObject(drops[1], transform.position, transform.rotation, ObjectPoolManager.PoolType.GoldNug);
            tempGoldDrop.GetComponent<MoveToPlayer>().amount = (int)_stats["gold"].Value;
            tempGoldDrop.GetComponent<MoveToPlayer>().visuals.transform.localScale = new Vector3(1,1,1);
            GetComponent<ShootReward>().ShootObject(GetComponent<EnemyMovementController>().visuals, tempGoldDrop, ShootReward.ShootType.Up);
        }

        xpDropChance = Random.Range(0,101);
        if(xpDropChance < baseXPDropChance)
        {
            GameObject tempDrop = ObjectPoolManager.SpawnObject(drops[0], transform.position, transform.rotation, ObjectPoolManager.PoolType.XpOrbParent);
            tempDrop.GetComponent<MoveToPlayer>().amount = (int)_stats["xp"].Value;
            tempDrop.GetComponent<MoveToPlayer>().visuals.transform.localScale = new Vector3(1,1,1);
            GetComponent<ShootReward>().ShootObject(GetComponent<EnemyMovementController>().visuals, tempDrop, ShootReward.ShootType.Up);
        }
        //spawn xp
    }

    public void SpawnLargeDrop()
    {
        int dropChance = Random.Range(0,101); 
        //Debug.Log(dropChance);
        if(dropChance <= 50)
        {
            GameObject tempGoldDrop = ObjectPoolManager.SpawnObject(drops[1], transform.position, transform.rotation, ObjectPoolManager.PoolType.GoldNug);
            _stats["gold"].Value = 10;
            tempGoldDrop.GetComponent<MoveToPlayer>().amount = (int)_stats["gold"].Value;
            tempGoldDrop.GetComponent<MoveToPlayer>().visuals.transform.localScale = new Vector3(2,2,2);
            tempGoldDrop.GetComponent<MoveToPlayer>().visuals.GetComponent<VisualEffects>().offset += 1;
            GetComponent<ShootReward>().ShootObject(GetComponent<EnemyMovementController>().visuals, tempGoldDrop, ShootReward.ShootType.Up);
        }
        else
        {
            GameObject tempXpDrop = ObjectPoolManager.SpawnObject(drops[0], transform.position, transform.rotation, ObjectPoolManager.PoolType.XpOrbParent);
            _stats["xp"].Value = 10;
            tempXpDrop.GetComponent<MoveToPlayer>().amount = (int)_stats["xp"].Value;
            tempXpDrop.GetComponent<MoveToPlayer>().visuals.transform.localScale = new Vector3(2,2,2);
            tempXpDrop.GetComponent<MoveToPlayer>().visuals.GetComponent<VisualEffects>().offset += 1;
            GetComponent<ShootReward>().ShootObject(GetComponent<EnemyMovementController>().visuals, tempXpDrop, ShootReward.ShootType.Up);
        }

        int relicDropChance = Random.Range(0,101);
        if(inExfilPhase)
        {
            if(relicDropChance <= 40)
            {
                //drop relic
                GameObject relicToSpawnObj = Instantiate(GameZoneController.Instance.relicLibrary.relicList[Random.Range(0,GameZoneController.Instance.relicLibrary.relicList.Count)], GetComponent<EnemyMovementController>().visuals.transform.position, Quaternion.identity);
                relicToSpawnObj.transform.parent = GameObject.FindGameObjectWithTag("RelicHolder").transform;
                GetComponent<ShootReward>().ShootObject(GetComponent<EnemyMovementController>().visuals, relicToSpawnObj, ShootReward.ShootType.Facing);
                    
            }
        }
        else
        {
            if(relicDropChance <= 80)
            {
                //drop relic
                GameObject relicToSpawnObj = Instantiate(GameZoneController.Instance.relicLibrary.relicList[Random.Range(0,GameZoneController.Instance.relicLibrary.relicList.Count)], GetComponent<EnemyMovementController>().visuals.transform.position, Quaternion.identity);
                relicToSpawnObj.transform.parent = GameObject.FindGameObjectWithTag("RelicHolder").transform;
                GetComponent<ShootReward>().ShootObject(GetComponent<EnemyMovementController>().visuals, relicToSpawnObj, ShootReward.ShootType.Facing);
                    
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
