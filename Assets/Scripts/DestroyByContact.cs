using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyByContact : MonoBehaviour {

    // 要產生的爆炸特效
    public GameObject explosion;
    public GameObject playerExplosion;

    // 摧毀隕石所得的分數
    public int scoreValue;

    // MainGameController腳本的參考
    private MainGameController gameController;

    void Start()
    {
        // 在開始時尋找TAG叫做Contorller的物件..
        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");

        // 如果有找到
        if (gameControllerObject != null)
        {
            // 腳本的參考設定為該物件的元件MainGameController,也就是我們自己寫的那個腳本
            gameController = gameControllerObject.GetComponent<MainGameController>();
        }
        else if (gameControllerObject == null)
        {
            Debug.Log("找不到MainGameController腳本");
        }

    }

    // 當他被任何物體碰到的時候
    void OnTriggerEnter(Collider other)
    {

        // 如果與之碰撞的是boundary 回傳null 並且結束此函示
        if (other.tag == "Boundary" || other.tag == "Enemies")
        {
            return;
        }

        

        // 玩家爆炸特效
        if (other.tag == "Player")
        {
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            gameController.GameOver();
        }

        // 使用該類別的公開方法AddScore來增加分數
        gameController.AddScore(scoreValue);


        // 建立爆炸特效
        Instantiate(explosion, transform.position, transform.rotation);

        if (other.tag != "Laser")
        {
            // 摧毀碰到此物件的物體
            Destroy(other.gameObject);
        }
        // 摧毀此物件
        Destroy(gameObject);


    }
}
