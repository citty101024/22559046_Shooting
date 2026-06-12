using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DropItem : MonoBehaviour
{
    public ItemData itemData;
    public int count = 1;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (PlayerInventory.Instance == null) return;
        bool added = PlayerInventory.Instance.AddItem(itemData, count);

        if (added)
        {
            Destroy(gameObject);
        }
        // Player 태그인지 확인
        // PlayerInventory.Instance.AddItem() 호출
        // 획득 성공 시 Destroy(gameObject)
    }
}
