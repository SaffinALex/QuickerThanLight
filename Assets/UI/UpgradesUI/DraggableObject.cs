﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class DraggableObject : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Canvas canvas;
    public GameObject upgrade;
    private RectTransform rect;
    private CanvasGroup canvasG;
    private Vector2 originalPos;

    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        canvasG = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("OnPointerDown");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("OnEndDrag");

        canvasG.alpha = 1f;
        canvasG.blocksRaycasts = true;

        rect.anchoredPosition = originalPos;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag");
        canvasG.alpha = 0.5f;
        canvasG.blocksRaycasts = false;
        originalPos = rect.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.anchoredPosition += eventData.delta / (canvas.scaleFactor * this.transform.parent.gameObject.GetComponent<RectTransform>().localScale.x);
    }

    public bool UpgradeIsOfType<ComponentType>() where ComponentType : UnityEngine.Component
    {
        return upgrade.GetComponent<ComponentType>() != null;
    }

    public bool ThisIsOfType<ComponentType>() where ComponentType : UnityEngine.Component
    {
        return upgrade.GetComponent<ComponentType>() != null;
    }

    public GameObject GetUpgrade()
    {
        return upgrade;
    }

    public void ResetPos()
    {
        if (originalPos != null)
            rect.anchoredPosition = originalPos;
    }

    public InventorySlot getInventorySlotParent()
    {
        return GetComponentInParent<InventorySlot>();
    }
}
