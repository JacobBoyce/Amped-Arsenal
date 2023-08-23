using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItemSelectionManager : MonoBehaviour
{
    public static ShopItemSelectionManager instance;

    public GameObject[] shopItems;

    [SerializeField]public GameObject LastSelected {get; set;}
    [SerializeField]public int LastSelectedIndex {get; set;}

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        // If we move right
        if(InputManager.instance.NavigationInput.x > 0)
        {
            Debug.Log("pressed Right");
            HandleNextUISelection(1);
        }

        // If we move left
        if(InputManager.instance.NavigationInput.x < 0)
        {
            Debug.Log("pressed Left");
            HandleNextUISelection(-1);
        }
    }

    public void OnEnable()
    {
        Debug.Log("enabled shop input manager");
        StartCoroutine(SetSelectedAfterOneFrame());
    }

    private IEnumerator SetSelectedAfterOneFrame()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(shopItems[0]);
    }

    private void HandleNextUISelection(int addition)
    {
        if(EventSystem.current.currentSelectedGameObject == null && LastSelected != null)
        {
            int newIndex = LastSelectedIndex + addition;
            newIndex = Mathf.Clamp(newIndex, 0, shopItems.Length-1);
            EventSystem.current.SetSelectedGameObject(shopItems[newIndex]);
        }
    }
}
