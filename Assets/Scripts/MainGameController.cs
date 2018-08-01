using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainGameController : MonoBehaviour {

    // 隕石物件參考
    public GameObject[] hazards;

    // 設定生成位置
    public Vector3 spawnValue;

    // 設定一波要生成幾個隕石
    public int hazardCount;

    // 生成每一顆的間隔時間
    public float spawnWait;

    // 開始生成前的小暫停時間
    public float firstStartWait;

    // 每一波隕石的間隔時間
    public float waveWait;

    // 記分板UI
    public Text scoreText;

    // 重新開始UI
    public Text restartText;
    private bool restart;

    // 遊戲結束UI
    public Text gameoverText;
    private bool gameover;

    // 分數
    private int score;


	// Use this for initialization
	void Start () {

        // 開始生成隕石
        StartCoroutine(SpawnWaves());

        // 重置記分板
        score = 0;
        UpdateScore();

        // 重置遊戲狀態
        restartText.text = "";
        gameoverText.text = "";
            
        restart = false;
        gameover = false;
	}

    void Update()
    {
        if (restart == true && Input.GetKeyDown(KeyCode.R))
        {
            // 影片方法已經過時 改用下面新方法
            // Application.LoadLevel(Application.loadedLevel);

            SceneManager.LoadScene(0);
        }
        


        SlowMotion();
    }

    // 生成隕石的自訂方法
    IEnumerator SpawnWaves()
    {
        // 開始此函式前暫停一個秒數
        yield return new WaitForSeconds(firstStartWait);

        // 讓他不斷生成隕石
        while (true)
        {
            // 一次生成數顆
            for (int i = 0; i < hazardCount; i++)
            {
                // 設定生成隕石位置
                Vector3 spawnPos = new Vector3(Random.Range(-spawnValue.x, spawnValue.x), spawnValue.y, spawnValue.z);

                // 宣告一個"沒有旋轉"的旋轉參數
                Quaternion spawnRotation = Quaternion.identity;

                int random = Random.Range(0, hazards.Length);

                // 生成隕石
                Instantiate(hazards[random], spawnPos, spawnRotation);

                // 迴圈跑一次會在這邊暫停一個短暫秒數 所以就不會一次生成一整排的隕石
                yield return new WaitForSeconds(spawnWait);
            }
            // 生成一波隕石後暫停數秒
            yield return new WaitForSeconds(waveWait);

            // 如果遊戲結束
            if (gameover)
            {
                // 顯示提示訊息
                restartText.text = "Press \"R\" To Restart";

                // 將狀態改為準備重新開始
                restart = true;

                // 離開生成隕石的迴圈
                break;
            }
        }
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    // 更新分數的自訂方法
    void UpdateScore()
    {
        scoreText.text = "目前分數: " + score;
    }

    // 結束遊戲
    public void GameOver()
    {
        gameoverText.text = "Game Over";
        gameover = true;
    }

    public void SlowMotion()
    {
        if (Input.GetButton("fun"))
        {
            if (Time.timeScale == 1.0f)
            {
                Time.timeScale = 0.5f;
                Time.fixedDeltaTime = 0.5f;
            }
                
        }
        else {
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = 1.0f;
        }
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
    }
}
