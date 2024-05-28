using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeapChoicePrefab : MonoBehaviour
{
    public Image weapImg, bgImg;
    public string weapName;
    [SerializeField]
    private Sprite deselected, selected;
    public bool isSelected = false;

    public void Select()
    {
        bgImg.sprite = selected;
        isSelected = true;
    }

    public void Deselect()
    {
        bgImg.sprite = deselected;
        isSelected = false;
    }
}
