using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float spd = 5.0f;
    Vector3 direct = Vector3.down;

    public GameObject prefabsExplosion;

    // Start is called before the first frame update
    void Start()
    {
        int rndNum = Random.Range(0, 10);

        if (rndNum % 3 == 0)
        {
            GameObject target = GameObject.Find("Character");
            direct = target.transform.position - transform.position;
            direct.Normalize();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position = transform.position + direct * spd * Time.deltaTime;
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject explosionObj = Instantiate(prefabsExplosion);
        explosionObj.transform.position = transform.position;
        Destroy(collision.gameObject);

        Destroy(gameObject);
    }
}
