using System.Net;
using UnityEngine;

public class MonsterDropper : MonoBehaviour
{
    [System.Serializable]
    public class DropTable
    {
        public ItemData itemData;
        [Range(0f, 1f)] public float dropRate = 0.5f;
        public int minCount = 1;
        public int maxCount = 1;
    }

    public GameObject dropPrefab;
    public DropTable[] dropTables;

    public void Drop()
    {
        // dropTables를 반복문으로 돌기
        if (dropPrefab == null || dropTables == null)
            return;
        foreach (DropTable table in dropTables)
        {
            if (table.itemData == null)
                continue;
            if (Random.value > table.dropRate)
                continue;

            GameObject dropObject = Instantiate(dropPrefab, transform.position, Quaternion.identity);
            DropItem dropItem = dropObject.GetComponent<DropItem>();

            if (dropItem != null)
            {
                dropItem.itemData = table.itemData;
                dropItem.count = Random.Range(table.minCount, table.maxCount + 1);
            }
        }
        // Random.value로 드랍 확률 검사

        // dropPrefab을 생성하고 DropItem에 데이터 넣기
        
    }
}
