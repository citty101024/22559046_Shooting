using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HPManager : MonoBehaviour
{
    public int hp = 100;
    public TextMeshProUGUI hpText;
    public GameObject MosterExplosion;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            hp -= 10;
            hpText.text = "HP : " + hp;

            Destroy(collision.gameObject);

            if (hp <= 0)
            {
                GameObject MexplosionObj = Instantiate(MosterExplosion);
                MexplosionObj.transform.position = transform.position;
                Destroy(gameObject);
            }
        }
    }
    public void Heal(int amount)
    {
        hp += amount;
        if (hp > 100) hp = 100;
        hpText.text = "HP : " + hp;
    }
}
