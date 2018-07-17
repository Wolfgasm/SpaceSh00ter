using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileMovement : MonoBehaviour {

    Transform target;

    Rigidbody missileRigidbody;

    

    public float speed;
    public float homingSpeed;

    public GameObject explosion;

    void Awake()
    {

        
    }
    // Use this for initialization
    void Start () {
        missileRigidbody = GetComponent<Rigidbody>();

        
    }
	
	// Update is called once per frame
	void Update () {
        target = GetClosestEnemy();
	}

    void FixedUpdate()
    {
        try
        {
            Vector3 direction = target.position - missileRigidbody.position;

            direction.Normalize();

            Vector3 rotate = Vector3.Cross(direction, transform.forward);

            missileRigidbody.angularVelocity = -rotate * homingSpeed;

            missileRigidbody.velocity = transform.forward * speed;

        }
        catch
        {
            //missileRigidbody.velocity = Vector3.forward * speed;
            missileRigidbody.velocity = transform.forward * speed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            return;
        }
        if (other.CompareTag("Boundary"))
        {
            return;
        }

        Instantiate(explosion, transform.position, transform.rotation);
    }

    // 尋找最近的敵人(物件)方法 距離的比較上都用次方去比 會比起用distance方法較省效能 免得unity背後還要幫開根號
    Transform GetClosestEnemy()
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

        // 計算距離的起始點 這裡是玩家
        Vector3 currentPosition = transform.position;

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
