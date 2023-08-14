using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystickController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public GameObject stick;
    public Camera cam;
    public Canvas canvas;
    void Start()
    {
        stick.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("clicked");
        stick.gameObject.SetActive(true);
        stick.GetComponent<RectTransform>().anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        OnDrag(eventData);
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        stick.gameObject.SetActive(false);
    }

    protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
    {
        Vector2 localPoint = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this.GetComponent<RectTransform>(), screenPosition, cam, out localPoint))
        {
            Vector2 pivotOffset = this.GetComponent<RectTransform>().pivot * this.GetComponent<RectTransform>().sizeDelta;
            return localPoint - (stick.GetComponent<RectTransform>().anchorMax * this.GetComponent<RectTransform>().sizeDelta) + pivotOffset;
        }
        return Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        cam = null;
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            cam = canvas.worldCamera;

        Vector2 position = RectTransformUtility.WorldToScreenPoint(cam, stick.GetComponent<RectTransform>().position);
    }
}
