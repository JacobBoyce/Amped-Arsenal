using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSelectionManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [TextArea]
    public string toolTip;
    [SerializeField] private readonly float _verticalMoveAmount = 30f;
    [SerializeField] private readonly float _moveTime = .1f;
    [Range(0f,2f), SerializeField] private readonly float _scaleAmount = 1.1f;
    private Vector3 _startPos, _startScale;

    // Start is called before the first frame update
    void Start()
    {
        _startPos = transform.position;
        _startScale = transform.localScale;
    }

    private IEnumerator MoveShopItem(bool startingAnimation)
    {
        Vector3 endPosition, endScale;
        float elapsedTime = 0f;

        while(elapsedTime < _moveTime)
        {
            elapsedTime += Time.unscaledDeltaTime;

            if(startingAnimation)
            {
                endPosition = _startPos + new Vector3(0f, _verticalMoveAmount, 0f);
                endScale = _startScale * _scaleAmount;
            }
            else
            {
                endPosition = _startPos;
                endScale = _startScale;
            }

            Vector3 lerpedPos = Vector3.Lerp(transform.position, endPosition, (elapsedTime / _moveTime));
            Vector3 lerpedScale = Vector3.Lerp(transform.localScale, endScale, (elapsedTime / _moveTime));

            //transform.position = lerpedPos;
            transform.localScale = lerpedScale;

            yield return null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventData.selectedObject = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        eventData.selectedObject = null;
    }

    public void OnSelect(BaseEventData eventData)
    {
        StartCoroutine(MoveShopItem(true));
        ShopItemSelectionManager.instance.LastSelected = gameObject;

        // Find the index
        for(int i = 0; i < ShopItemSelectionManager.instance.shopItems.Length; i++)
        {
            if(ShopItemSelectionManager.instance.shopItems[i] == gameObject)
            {
                ShopItemSelectionManager.instance.LastSelectedIndex = i;
                ShopItemSelectionManager.instance.ShowToolTip(toolTip);
                return;
            }
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        ShopItemSelectionManager.instance.ShowToolTip("");
        StartCoroutine(MoveShopItem(false));
    }
}
