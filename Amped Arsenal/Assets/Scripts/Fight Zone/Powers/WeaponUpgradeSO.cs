using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "###Weapon_Upgrade", menuName = "Weapon Upgrade")]
public class WeaponUpgradeSO : ScriptableObject
{
    [SerializeField]
    public List<WeapUpgrade> UpgradeList = new();
    public int[] costValues = new int[5];
    
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
        PIERCE,
        RANGE
    };
    public WeaponUpgrade weapUpType;
    //public Sprite upImg;

    public float curVal;

    [Space(10)]
    public int[] upValues = new int[5];
}
