using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EquippedHUD : MonoBehaviour
{
    public List<Image> equippedUIImage = new();
    public int curNum = 0;

    public void Start()
    {
        for(int i = 0; i < equippedUIImage.Count; i++)
        {
            equippedUIImage[i].gameObject.SetActive(false);
        }
    }
    
    public void ApplyImage(Sprite img)
    {
         equippedUIImage[curNum].gameObject.SetActive(true);
        equippedUIImage[curNum].sprite = img;
        curNum++;
    }
}
