using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public HPManager hpManager;
    public void OnBagItemClicked(InventoryItem item, int index)
    {
        if (item == null || item.data == null) return;

        Debug.Log($"InventoryUI에서 클릭 처리: {item.data.itemName}");

        if (hpManager != null)
        {
            hpManager.Heal(3);
        }

        if (PlayerInventory.Instance == null) return;
        PlayerInventory.Instance.RemoveOneBagItem(index);

        Refresh();
    }

    public GameObject inventoryPanel;
    public InventorySlotUI[] bagSlots;
    public InventorySlotUI[] equipSlots;
 
    private void Start()
    {
        inventoryPanel.SetActive(false);
        Refresh();
    }

    public void Toggle()
    {
        Debug.Log("Toggle 버튼 눌렸습니다!");
        // 패널 열기/닫기
        if (inventoryPanel == null)
        {
            Debug.LogWarning("Inventory Panel이 연결되지 않았습니다.");
            return;
        }
        // 열릴 때 Refresh() 호출
        bool nextOpen = !inventoryPanel.activeSelf;
        if (nextOpen)
        {
            Refresh();
        }

        inventoryPanel.SetActive(!inventoryPanel.activeSelf);

    }

    public void Refresh()
    {
        // bagSlots와 equipSlots에 PlayerInventory의 리스트 연결
        PlayerInventory inventory = PlayerInventory.Instance;

        if (inventory == null)
        {
            Debug.LogWarning("Player Inventory.Instance가 없습니다.");
            return;
        }

        for (int i = 0; i < bagSlots.Length; i++)
        {
            bagSlots[i].SetSlot(this, inventory.bagItems, i);
        }

        for (int i = 0; i < equipSlots.Length; i++)
        {
            equipSlots[i].SetSlot(this, inventory.equipItems, i);
        }
    }

    
}
