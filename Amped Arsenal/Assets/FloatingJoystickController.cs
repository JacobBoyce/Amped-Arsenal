using System.Collections;
using System.Collections.Generic;
using Den.Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;

public class FloatingJoystickController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public GameObject stick;
    public Camera cam;
    public Canvas canvas;
    public OnScreenStick onScreenStickController;
    public Image stickImage;
    public bool interupted;
     void Start()
    {
        //stickImage = GetComponentInChildren<Image>();
        stickImage.enabled = false;
        //stick.gameObject.SetActive(false);
         cam = null;
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            cam = canvas.worldCamera;

    }

    void OnEnable()
    {
        //stick.gameObject.SetActive(false);
        stickImage.enabled = false;
        onScreenStickController.enabled = true;
        interupted = false;
        // /onScreenStickController.interrupted = true;
    }

    void OnDisable()
    {
        interupted = true;
        onScreenStickController.enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("clicked");
        //stick.gameObject.SetActive(true);
        stick.GetComponent<RectTransform>().anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        stickImage.enabled = true;
        OnDrag(eventData);
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        stickImage.enabled = false;
        //stick.gameObject.SetActive(false);
    }

    public Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
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
        Vector2 position = RectTransformUtility.WorldToScreenPoint(cam, stick.GetComponent<RectTransform>().position);
    }
}
