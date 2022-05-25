using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour, IWeapon
{
    public string wName;
    public GameObject weapPrefab;
    public PlayerController playerObj;
    public ShopItemSO shopItemInfo;
    public WeaponMods weapMod;
    public int curCooldown, tickMaxCD, damage;

    [Header("Spawnpoint:\n0 = Front\n1 = Back\n2 = Left\n3 = Right \n4 = Center")]
    [SerializeField]
    public List<SpawnDeets> spawnDetails = new List<SpawnDeets>();

    [Space(10)]
    public int level = 1;

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
