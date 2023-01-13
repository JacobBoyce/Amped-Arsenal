using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour, IWeapon
{
    public string wName;
    public GameObject weapPrefab;
    public PlayerController playerObj;
    public ShopItemSO shopItemInfo;
    public WeaponUpgradeSO weapUpgrades;
    public WeaponMods weapMod;
    public int curCooldown, tickMaxCD, damage;

    public int maxSlots = 1, currentEquippedSlots = 0;
    public List<GameObject> effectSlots = new List<GameObject>();

    [Header("Spawnpoint:\n0 = Front\n1 = Back\n2 = Left\n3 = Right \n4 = Center")]
    [SerializeField]
    public List<SpawnDeets> spawnDetails = new List<SpawnDeets>();

    [Space(10)]
    public int level = 1, maxLevel = 5;

    public bool IsMaxLvl()
    {
        return level == maxLevel ? true : false;
    }

    public void AddEffectToWeapon(GameObject effectPrefab)
    {
        effectSlots.Add(effectPrefab);
        currentEquippedSlots++;
    }
    public void ApplyEffects(EnemyController enemy)
    {
        foreach(GameObject eff in effectSlots)
        {
            float rand = Random.Range(0.0f,1.0f);
            //Debug.Log(playerObj._stats["luck"].Value / playerObj._stats["luck"].Max + " >= " + rand);

            if(playerObj._stats["luck"].Value / playerObj._stats["luck"].Max >= rand)
            {
                //Debug.Log("apply: " + eff.GetComponent<EffectBase>().effectName);
                //check if can apply new or update cd of effect
                if(enemy.HasEffect(eff.GetComponent<EffectBase>()))
                {
                    //update cooldown
                    //Debug.Log("Enemy is afflicted with - " + eff.GetComponent<EffectBase>().effectName);
                    //effect controller method call for updateCooldown
                    enemy.UpdateEffect(eff);
                }
                else
                {
                    //instastiate effect base effectPrefab
                    //Instantiate();
                    //Debug.Log("Enemy is NOT afflicted with - " + eff.GetComponent<EffectBase>().effectName);
                    enemy.AddEffect(eff);
                    //eff.GetComponent<EffectBase>().CallEffect(enemy, this);
                }
            }
        }
    }

    public virtual void ActivateAbility()
    {

    }

    public virtual void UpgradeWeapon()
    {

    }

    public virtual void SetSpawnDetails()
    {

    }
}

[System.Serializable]
public struct SpawnDeets
{
    [SerializeField]
    public int spawnpoint;
    [SerializeField]
    public bool needsParent;
}
