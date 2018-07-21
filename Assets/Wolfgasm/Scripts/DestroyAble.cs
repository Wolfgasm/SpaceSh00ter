using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAble : MonoBehaviour {
    // 此物體的血量
    public int health;

    // 此物體死掉後的特效
    public GameObject deathEffect;

    // 此物體能給予的分數
    public int scoreValue;

    // Destroyer腳本
    private Destroyer destroyer;

    // 遊戲控制器腳本
    private MainGameController mainGameController;

	// Use this for initialization
	void Start () {
        mainGameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<MainGameController>();
	}
	
	// Update is called once per frame
	void LateUpdate () {

        // 如果血量小於0
        if (health <= 0)
        {
            if (this.gameObject.tag == "Player")
            {
                mainGameController.GameOver();
            }
            health = 0;
            Instantiate(deathEffect, transform.position, transform.rotation);
            mainGameController.AddScore(scoreValue);
            Destroy(this.gameObject);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        // 如果與之碰撞的是子彈 並且此物件不是玩家的話
        if (other.tag == "Bullet" && this.gameObject.tag!="Player")
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

        // 如果碰到的是玩家 而且此角本附掛的物件是敵人
        if (other.tag == "Enemies" && this.gameObject.tag == "Player")
        {
            Debug.Log("NIGGER");
            destroyer = other.GetComponent<Destroyer>();

            health -= destroyer.damage;

            // 如果該子彈有附加特效
            if (destroyer.hitEffect != null)
            {


                // 產生該特定子彈的特效
                Instantiate(destroyer.hitEffect, new Vector3(other.transform.position.x, transform.position.y, transform.position.z), destroyer.hitEffect.transform.rotation);


            }

            // 刪除碰撞物
            Destroy(other.gameObject);
        }
        
    }
}
