using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUpgradeLibrary : MonoBehaviour
{
    [SerializeField]
    public List<UpgradeUX> upUX = new();
}

[System.Serializable]
public class UpgradeUX
{
    public string name;
    public Sprite upUI;
}
