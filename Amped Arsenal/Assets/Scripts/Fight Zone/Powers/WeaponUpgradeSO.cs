using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "###Weapon_Upgrade", menuName = "Weapon Upgrade")]
public class WeaponUpgradeSO : ScriptableObject
{
    [SerializeField]
    public List<WeapUpgrade> UpgradeList = new();
}

[System.Serializable]
public class WeapUpgrade
{
    public enum WeaponUpgrade
    {
        NONE,
        DAMAGE,
        DURATION,
        COOLDOWN,
        AMOUNT,
        SPEED,
        PIERCE
    };
    public WeaponUpgrade weapUp;

    public float val;

    [Space(10)]
    public int[] upValues = new int[5];
}
