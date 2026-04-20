using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFire : MonoBehaviour
{
    public GameObject bulletObject;
    public GameObject bulletFireObject;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool isFire = Input.GetButtonDown("Jump");
        if (isFire)
        {
            anim = GameObject.Find("LittleCat").GetComponent<Animator>();
            anim.SetTrigger("Throw");
            GameObject bullet = Instantiate(bulletObject);
            bullet.transform.position = bulletFireObject.transform.position;
        }
    }
}
