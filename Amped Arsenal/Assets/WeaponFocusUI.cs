using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponFocusUI : MonoBehaviour
{
    public Image upgradeWeaponImg;
    public TextMeshProUGUI lvlUpFrom, lvlUpTo;

    public GameObject upgradeSlotParent, statsUpSlotPrefab;
    private StatUpInfoSlot tempSlot;

    public List<GameObject> upInfoSlot = new();
}
