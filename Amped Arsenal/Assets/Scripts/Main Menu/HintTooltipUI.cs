using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class HintTooltipUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public GameObject displayFieldObj;
    public TextMeshProUGUI displayTxt;
    public string tipToDisplay;



    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        UpdateText(true);
    }
    public void OnDeselect(BaseEventData eventData)
    {
        UpdateText(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UpdateText(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UpdateText(false);
    }

    public void UpdateText(bool show)
    {
        displayFieldObj.SetActive(show);
        displayTxt.text = tipToDisplay;
    }
}
