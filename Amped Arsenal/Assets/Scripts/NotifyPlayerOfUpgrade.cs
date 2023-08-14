using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotifyPlayerOfUpgrade : MonoBehaviour
{
    public GameObject normalUpButton, activeUpButton;

    public void ChangeButtonImage(bool toggle)
    {
        normalUpButton.SetActive(!toggle);
        activeUpButton.SetActive(toggle);
    }
}
