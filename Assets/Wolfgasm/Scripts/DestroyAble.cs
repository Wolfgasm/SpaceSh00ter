using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAble : MonoBehaviour {
    // 此物體的血量
    public int health;

    // 此物體死掉後的特效
    public GameObject deathEffect;

    // Destroyer腳本
    private Destroyer destroyer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        // 如果血量小於0
        if (health <= 0)
        {
            health = 0;
            Instantiate(deathEffect, transform.position, transform.rotation);

            Destroy(this.gameObject);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        // 如果與之碰撞的是子彈
        if (other.tag == "Bullet")
        {
            // 先尋找Destoyer類別
            destroyer = other.GetComponent<Destroyer>();

            // 利用Destroyer的屬性damage扣減此物體的血量
            health -= destroyer.damage;

            
            // 如果該子彈有附加特效
            if (destroyer.hitEffect != null)
            {
                

                // 產生該特定子彈的特效
                Instantiate(destroyer.hitEffect, new Vector3(other.transform.position.x,transform.position.y,transform.position.z) , destroyer.hitEffect.transform.rotation);


            }

            // 刪除碰撞物
            Destroy(other.gameObject);
        }

    }
}
