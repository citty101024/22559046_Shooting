using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
    public Image iconImage;
    public TMP_Text countText;

    private InventoryUI inventoryUI;
    private List<InventoryItem> itemList;
    private int index;

    public void SetSlot(InventoryUI inventoryUI, List<InventoryItem> itemList, int index)
    {
        this.inventoryUI = inventoryUI;
        this.itemList = itemList;
        this.index = index;
        RefreshView();
    }

    public void RefreshView()
    {
        if (itemList == null || index < 0 || index >= itemList.Count)
        {
            ClearView();
            return;
        }
        InventoryItem item = itemList[index];
        if (item == null || item.data == null)
        {
            ClearView();
            return;
        }

        iconImage.enabled = true;
        iconImage.sprite = item.data.icon;
        iconImage.color = Color.white;
        iconImage.raycastTarget = true;

        if (item.count > 1)
        {
            countText.text = item.count.ToString();
        }
        else
        {
            countText.text = "";
        }
    }

    private void ClearView()
    {
        if (iconImage != null)
        {
            iconImage.enabled = false;
            iconImage.sprite = null;
            iconImage.raycastTarget = true;
        }

        if (countText != null)
        {
            countText.text = "";
        }
    }

    private RectTransform iconRect;
    private Vector3 iconStartPosition;
    private Canvas rootCanvas;
    private Transform originalParent;
    private int originalSiblingIndex;

    private void Awake()
    {
        if (iconImage != null)
        {
            iconRect = iconImage.GetComponent<RectTransform>();
            iconImage.raycastTarget = false;
        }

        if (countText != null)
        {
            countText.raycastTarget = false;
        }

        rootCanvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemList == null || index < 0 || index >= itemList.Count) return;
        if (itemList[index] == null || itemList[index].data == null) return;
        if (iconRect == null) return;
        if (rootCanvas == null) return;

        UnityEngine.Debug.Log("드래그 시작: " + itemList[index].data.itemName);

        iconStartPosition = iconRect.position;
        originalParent = iconRect.parent;
        originalSiblingIndex = iconRect.GetSiblingIndex();

        iconRect.SetParent(rootCanvas.transform, true);
        iconRect.SetAsLastSibling();

        if (iconImage != null)
        {
            iconImage.raycastTarget = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemList == null || index < 0 || index >= itemList.Count) return;
        if (itemList[index] == null || itemList[index].data == null) return;
        if (iconRect == null) return;

        iconRect.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        UnityEngine.Debug.Log("드래그 종료");

        if (iconRect != null && originalParent != null)
        {
            iconRect.SetParent(originalParent, true);
            iconRect.SetSiblingIndex(originalSiblingIndex);
            iconRect.anchoredPosition = Vector2.zero;
        }

        if (iconRect != null)
        {
            inventoryUI.Refresh();
        }

        if (iconImage != null)
        {
            iconImage.raycastTarget = false;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventorySlotUI fromSlot = eventData.pointerDrag.GetComponentInParent<InventorySlotUI>();

        if (fromSlot == null)
        {
            UnityEngine.Debug.LogWarning("드래그 시작 슬롯을 찾지 못했습니다.");
            return;
        }
        if (fromSlot == this) return;
        if (PlayerInventory.Instance == null)
        {
            UnityEngine.Debug.LogWarning("PlayerInventory.Instance가 없습니다.");
            return;
        }

        UnityEngine.Debug.Log("드롭 성공");

        PlayerInventory.Instance.MoveItem(
            fromSlot.itemList,
            fromSlot.index,
            this.itemList,
            this.index
        );

        if (inventoryUI != null) inventoryUI.Refresh();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (itemList == null || index < 0 || index >= itemList.Count) return;
        InventoryItem item = itemList[index];

        if (item == null || item.data == null) return;
        if (PlayerInventory.Instance == null) return;
        if (itemList != PlayerInventory.Instance.bagItems) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            UnityEngine.Debug.Log($"Bag 아이템 클릭: {item.data.itemName} / 개수: {item.count}");
            if (inventoryUI != null)
            {
                inventoryUI.OnBagItemClicked(item, index);
            }
        }
    }
}