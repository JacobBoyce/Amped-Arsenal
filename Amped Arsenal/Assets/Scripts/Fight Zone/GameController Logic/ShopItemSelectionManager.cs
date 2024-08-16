using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItemSelectionManager : MonoBehaviour
{
    public static ShopItemSelectionManager instance;

    public SelectedSoundMaker soundMaker;

    public GameObject[] shopItems;
    public TextMeshProUGUI tooltipText;

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
            HandleNextUISelection(1);
        }

        // If we move left
        if(InputManager.instance.NavigationInput.x < 0)
        {
            HandleNextUISelection(-1);
        }
    }

    public void OnEnable()
    {
        soundMaker.allowedToMakeNoise = true;
        foreach(GameObject go in shopItems)
        {
            go.GetComponent<UISelectableObject>().isSelected = false;
            go.GetComponent<UISelectableObject>().SetNormal(false);
            try{
                go.GetComponent<BaseUpgradeSquare>().isSelected = false;
            }
            catch{
                //Debug.Log("");
            }
        }
        StartCoroutine(SetSelectedAfterOneFrame());
    }

    public void OnDisable()
    {
        soundMaker.allowedToMakeNoise = false;
    }

    private IEnumerator SetSelectedAfterOneFrame()
    {
        soundMaker.allowedToMakeNoise = true;
        shopItems[0].GetComponent<UIButtonSelectionManager>().isOpened = true;
        yield return null;
        EventSystem.current.SetSelectedGameObject(shopItems[0]);
    }
    public IEnumerator SetSelectedAfterOneFrame(int index, bool wantFXPlayed)
    {
        shopItems[0].GetComponent<UIButtonSelectionManager>().isOpened = wantFXPlayed;
        yield return null;
        EventSystem.current.SetSelectedGameObject(shopItems[index]);
    }

    private void HandleNextUISelection(int addition)
    {
        if(EventSystem.current.currentSelectedGameObject == null && LastSelected != null)
        {
            int newIndex = LastSelectedIndex + addition;
            newIndex = Mathf.Clamp(newIndex, 0, shopItems.Length-1);
            EventSystem.current.SetSelectedGameObject(shopItems[newIndex]);
            shopItems[LastSelectedIndex].GetComponent<BaseUpgradeSquare>()?.SetUnFocused();
            shopItems[LastSelectedIndex].GetComponent<BuyButtonLogic>()?.BuyButtonUnSelected();
            shopItems[newIndex].GetComponent<BaseUpgradeSquare>()?.SetHover();
        }
    }
}
