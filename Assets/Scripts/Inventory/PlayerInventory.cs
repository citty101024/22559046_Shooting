using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    public int bagSlotCount = 3;
    public int equipSlotCount = 3;

    public List<InventoryItem> bagItems = new List<InventoryItem>();
    public List<InventoryItem> equipItems = new List<InventoryItem>();

    private void Awake()
    {
        Instance = this;
        // bagItems와 equipItems를 슬롯 수만큼 null로 채우기
        bagItems.Clear();
        equipItems.Clear();

        FillEmptySlots(bagItems, bagSlotCount);
        FillEmptySlots(equipItems, equipSlotCount);
    }

    private void FillEmptySlots(List <InventoryItem> list, int slotCount)
    {
        while (list.Count < slotCount)
        {
            list.Add(null);
        }
    }

    public bool AddItem(ItemData itemData, int count = 1)
    {
        if (itemData == null) return false;
        if (count <= 0) return false;

        if (itemData.canStack)
        {
            for (int i = 0; i < bagItems.Count; i++)
            {
                InventoryItem item = bagItems[i];

                if (item != null && item.data == itemData && item.count < itemData.maxStack)
                {
                    int addCount = Mathf.Min(count, itemData.maxStack - item.count);
                    item.count += addCount;
                    count -= addCount;

                    if (count <= 0)
                    {
                        Debug.Log(itemData.itemName + " 스택 추가 성공");
                        return true;
                    }
                }
            }
        }

        for (int i = 0; i < bagItems.Count; i++)
        {
            if ( bagItems[i] == null || bagItems[i].data == null)
            {
                int addCount = itemData.canStack ? Mathf.Min(count, itemData.maxStack) : 1;
                bagItems[i] = new InventoryItem(itemData, addCount);
                count -= addCount;

                Debug.Log(itemData.itemName + " 새 슬롯에 추가 성공 ");

                if (count <= 0)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void MoveItem(List<InventoryItem> fromList, int fromIndex, List<InventoryItem> toList, int toIndex)
    {
        // from 슬롯과 to 슬롯의 아이템을 서로 바꾸기
        if (!IsValidIndex(fromList, fromIndex) || !IsValidIndex(toList, toIndex)) return;
        InventoryItem fromItem = fromList[fromIndex];
        if(IsEmpty(fromItem)) return;

        bool isBagToEquip = fromList == bagItems && toList == equipItems;
        bool isEquipToBag = fromList == equipItems && toList == bagItems;

        if (isBagToEquip)
        {
            MoveOneItemToEquip(fromIndex, toIndex);
            return;             
        }


        if (isEquipToBag)
        {
            MoveEquipItemToBag(fromIndex, toIndex);
            return;
        }

        InventoryItem temp = toList[toIndex];
        toList[toIndex] = fromList[fromIndex];
        fromList[fromIndex] = temp;
    }

    private void MoveOneItemToEquip(int bagIndex, int equipIndex)
    {
        InventoryItem bagItem = bagItems[bagIndex];
        if (IsEmpty(bagItem)) return;
        if (!IsEmpty(equipItems[equipIndex]))
        {
            Debug.Log("장착 슬롯이 이미 사용 중입니다.");
            return;
        }
        ItemData itemData = bagItem.data;

        equipItems[equipIndex] = new InventoryItem(itemData, 1);
        bagItem.count--;
        if (bagItem.count <= 0)
        {
            bagItems[bagIndex] = null;
        }
        Debug.Log(itemData.itemName + " 1개 장착");
    }
    private bool IsEmpty(InventoryItem item)
    {
        return item == null || item.data == null || item.count <= 0;
    }

    private void MoveEquipItemToBag(int equipIndex, int bagIndex)
    {

        InventoryItem equipItem = equipItems[equipIndex];
        if (IsEmpty(equipItem)) return;
        InventoryItem bagItem = bagItems[bagIndex];

        if (IsEmpty(bagItem))
        {
            bagItems[bagIndex] = new InventoryItem(equipItem.data, equipItem.count);
            equipItems[equipIndex] = null;
            return;
        }
        if (bagItem.data == equipItem.data && bagItem.data.canStack) 
        {
            int space = bagItem.data.maxStack - bagItem.count;
            int addCount = Mathf.Min(space, equipItem.count);
            bagItem.count += addCount;
            equipItem.count -= addCount;


            if (equipItem.count <= 0)
            {
                equipItems[equipIndex] = null;
            }

            Debug.Log($"{bagItem.data.itemName}가방에 {addCount}개 합침");

            return;
        }
        
        InventoryItem temp = bagItems[bagIndex];

        bagItems[bagIndex] = new InventoryItem(equipItem.data, equipItem.count);
        equipItems[equipIndex] = temp; 

    }

    private bool IsValidIndex(List<InventoryItem> list, int index)
    {
        return list != null && index >= 0 && index < list.Count;
    }

    public void RemoveOneBagItem(int bagIndex)
    {
        if (!IsValidIndex(bagItems, bagIndex)) return;
        InventoryItem item = bagItems[bagIndex];

        if (item == null || item.data == null) return;
        item.count--;
        Debug.Log($"{item.data.itemName} 1개 사용");

        if (item.count <= 0)
        {
            bagItems[bagIndex] = null;
        }
    }
}

