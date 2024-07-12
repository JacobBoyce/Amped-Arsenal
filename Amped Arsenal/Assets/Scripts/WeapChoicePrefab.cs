using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeapChoicePrefab : MonoBehaviour
{
    public Image weapImg, bgImg;
    public string weapName;
    public bool isSelected = false;

    // void Awake()
    // {
    //     GetComponent<UIInteracableObjectVisuals>().OnObjectHovered += Hovered;
    // }
    // void OnDestroy()
    // {
    //     GetComponent<UIInteracableObjectVisuals>().OnObjectHovered -= Hovered;
    // }

    // public void Hovered()
    // {

    // }

    public void Select()
    {
        //bgImg.sprite = selected;
        //GetComponent<UIInteracableObjectVisuals>().SetGreen(false);
        //isSelected = true;
    }

    public void Deselect()
    {
        //GetComponent<UIInteracableObjectVisuals>().SetNormal(false);
        //GetComponent<UIInteracableObjectVisuals>().SetUnselected();
        //bgImg.sprite = deselected;
    }
}
