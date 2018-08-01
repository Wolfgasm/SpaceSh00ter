using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileMovement : MonoBehaviour {

    private Rigidbody missileRigidbody;

    // 追蹤目標
    private Transform target;

    // 玩家物件 用來定義距離最近敵人的起始點
    private GameObject player;

    // 飛行速度以及追蹤轉向速度
    public float speed;
    public float homingSpeed;

    // 爆炸特效
    public GameObject explosion;

    // Use this for initialization
    void Start () {
        missileRigidbody = GetComponent<Rigidbody>();

        // 在生成時尋找玩家
        player = GameObject.FindGameObjectWithTag("Player");

        // 如果找不到 將自身設為追蹤起始點
        if (player == null)
        {
            player = this.gameObject;
        }

    }
	
	// Update is called once per frame
	void Update () {
        // 尋找最近的敵人
        target = GetClosestEnemy();
    }

    void FixedUpdate()
    {
        // 如果有找到敵人
        try
        {
            // 計算敵人跟自己的位置向量
            Vector3 direction = target.position - missileRigidbody.position;

            // 將該向量標準化方便自訂轉向速度
            direction.Normalize();

            // 計算自身向量與敵人位置向量的外積
            Vector3 rotate = Vector3.Cross(direction, transform.forward);

            // 將角向量指定為向量外積的"負數"(否則會往反方向轉)
            missileRigidbody.angularVelocity = -rotate * homingSpeed;

            // 給予該鋼體一個持續往前(local z軸)的速度
            missileRigidbody.velocity = transform.forward * speed;

        }
        // 如果場上沒有敵人
        catch
        {
            //missileRigidbody.velocity = Vector3.forward * speed;
            missileRigidbody.velocity = transform.forward * speed;
        }

        // 讓飛彈會自體旋轉 雖然不重要
        missileRigidbody.angularVelocity += transform.forward * 5;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 如果撞擊到的是敵人
        if (other.CompareTag("Enemies"))
        {
            // 爆炸特效
            Instantiate(explosion, transform.position, transform.rotation);
        }
    }

    // 尋找最近的敵人(物件)方法 
    public Transform GetClosestEnemy()
    {
        GameObject[] enemies;

        Transform[] enemiesTrans;

        // 先尋找周圍的敵人
        enemies = GameObject.FindGameObjectsWithTag("Enemies");

        // 宣告一個Transform陣列儲存周圍的敵人位置
        enemiesTrans = new Transform[enemies.Length];
   
        // 將周圍敵人的Gameobject轉為Transform存入敵人位置陣列
        for (int i = 0; i < enemies.Length; i++)
        {
            enemiesTrans[i] = enemies[i].transform;
        }


       // 儲存最近的敵人位置
       Transform bestTarget = null;

        // 儲存最近敵人的距離長度 
        float closestDistanceSqr = Mathf.Infinity;

        // 儲存計算距離的起始點 這裡是玩家
        Vector3 currentPosition;

        // 如果玩家還存在
        if (player != null)
        {
            // 計算距離的原點設為玩家
            currentPosition = transform.position;
        }
        // 否則
        else {
            // 計算距離的原點設為飛彈本身
            currentPosition = transform.position;
        }
        
        // 逐一計算敵人位置陣列的每個內容
        foreach (Transform potentialTarget in enemiesTrans)
        {
            // 敵人與起始點的位置差
            Vector3 directionToTarget = potentialTarget.position - currentPosition;

            // 儲存位置差
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            // 比較目前的元素是否是最近的敵人
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        // 回傳結果
        return bestTarget;
    }

}
