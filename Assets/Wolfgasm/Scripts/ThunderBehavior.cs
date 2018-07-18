using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderBehavior : MonoBehaviour {

    private LineRenderer theLine;
    public int maximumTarget;

	// Use this for initialization
	void Start () {
        theLine = GetComponent<LineRenderer>();
        //StartCoroutine(DestroysStuff());
    }
	
	// Update is called once per frame
	void Update () {

        theLine.SetPosition(0, transform.position);
        for (int i = 0; i < GetNearbyEnemy().Length; i++)
        {
            theLine.SetPosition(i + 1, GetNearbyEnemy()[i].transform.position);
        }


        GetNearbyEnemy();
       
        
        //Debug.Log(GetNearbyEnemy());
        
	}

    // 尋找最近的敵人(物件)方法 
    public GameObject[] GetNearbyEnemy()
    {
        GameObject[] allEnemies;

        // 宣告一個陣列存放減少長度後的後的敵人物件
        GameObject[] enemies;


        // 重要的變數 用來建立陣列或迴圈計算 除非敵人不足數 否則該值等於公開變數maximumTarget(自行設定的攻擊目標數)
        int maxTarget = maximumTarget;

        // 如果場上的敵人小於最大目標值
        if (GameObject.FindGameObjectsWithTag("Enemies").Length < maxTarget)
        {
            // 最大目標值等於場上目前敵人數 (因為最大目標值在後面都用來建立陣列之類的 如果不足數量那個元素就會是null,unity會報錯)
            maxTarget = GameObject.FindGameObjectsWithTag("Enemies").Length;
        }

        // 將Line Renderer的端點數量設為maxTarget 避免在敵人數量少的時候亂連一通
        theLine.positionCount = maxTarget + 1;

        // 先尋找周圍所有的敵人物件
        allEnemies = GameObject.FindGameObjectsWithTag("Enemies");








        // 用來儲存計算距離的起始點 這裡是腳本附掛的物件本身位置
        Vector3 currentPosition;
        currentPosition = this.transform.position;        

        for (int i = 0; i < allEnemies.Length; i++)
        {
            // 敵人與起始點的向量
            Vector3 directionToTarget = allEnemies[i].transform.position - currentPosition;

            // 將向量轉為距離
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            for (int j = 0; j < allEnemies.Length; j++)
            {
                GameObject temp;

                // 其他元素的敵人與起始點的向量
                Vector3 directionToTarget2 = allEnemies[j].transform.position - currentPosition;

                // 將向量轉為距離
                float dSqrToTarget2 = directionToTarget2.sqrMagnitude;

                // 排序由小到大
                if (dSqrToTarget < dSqrToTarget2)
                {
                    
                    temp = allEnemies[i];
                    allEnemies[i] = allEnemies[j];
                    allEnemies[j] = temp;

                }

            }
        }

        enemies = new GameObject[maxTarget];
        for (int i = 0; i < maxTarget; i++)
        {
            enemies[i] = allEnemies[i];
        }

        // 回傳排序後的敵人位置
        return enemies;
    }

    IEnumerator DestroysStuff()
    {
        yield return new WaitForSeconds(0);

        while (true)
        {
            foreach (GameObject stuff in GetNearbyEnemy())
            {
                Destroy(stuff);
                Debug.Log("刪除");
            }
            yield return new WaitForSeconds(5);
        }
    }

}
