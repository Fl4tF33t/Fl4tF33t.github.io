using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDragAndDropManager : Singleton<UIDragAndDropManager>, IDragHandler, IPointerClickHandler, IBeginDragHandler, IEndDragHandler
{
    internal UIShopButton uiShopButton;
    private Canvas canvas;

    protected override void Awake()
    {
        base.Awake();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (uiShopButton != null)
        {
            uiShopButton.canvasGroup.alpha = .6f;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (uiShopButton != null) 
        {
            uiShopButton.rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (uiShopButton != null)
        {
            uiShopButton.canvasGroup.alpha = 1f;
            //need a way to implement the return of ui elemenets that arent being held ddown by the finger
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked");
    }
   
}
