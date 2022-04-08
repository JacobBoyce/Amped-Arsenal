using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour, IWeapon
{
    public string wName;
    public GameObject weapPrefab;

    public int curCooldown, tickMaxCD;

    [Header("Spawnpoint:\n0 = Front\n1 = Back\n2 = Left\n3 = Right")]
    [SerializeField]
    public List<SpawnDeets> spawnDetails = new List<SpawnDeets>();

    public virtual void ActivateAbility()
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
