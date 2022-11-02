using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeapItemSlotUI : MonoBehaviour
{
    public int indentity;
    public string weapName;
    public GameObject selectBorder;
    public Image weapImg;
    public GameObject weapLvlBadge;
    public TextMeshProUGUI lvl;

    public void ClearStuff()
    {
        weapName = "";
        selectBorder.SetActive(false);
        weapImg.enabled = false;
        weapLvlBadge.SetActive(false);
        lvl.text = "";
    }
}
