using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// 自訂類別Boundary
[System.Serializable] //設定為可序列化物件 才能在unity的inspector中設定他的值
public class Boundary
{
    // x軸與z軸移動的限制值
    public float xMax, xMin;  
    public float zMin, zMax;

}


public class PlayerController : MonoBehaviour {

    // 玩家的rigidbody
    Rigidbody playerRigidbody;

    // 玩家的Audio Source
    AudioSource playerAudioSource;

    // 移動速度
    public float speed;

    // 旋轉角度
    public float tilt;


    // 基礎武器屬性
    public GameObject shot;         // 子彈物體
    public Transform shotSpawn;     // 子彈生成位置
    public float fireRate;          // 子彈射擊速度
    private float nextFire = 0.0f;  // 子彈下一次能射擊的時間

    // 機槍武器屬性
    Vector3 shotSpawnGatlin;                // 子彈生成位置
    public float fireRateGatlin;            // 子彈射擊速度
    private float nextFireGatlin = 0.0f;    // 子彈下一次能射擊的時間

    // 雷射武器屬性
    public GameObject laser;

    // 火箭
    public GameObject missile;
    public float fireRateMissile;            // 子彈射擊速度
    private float nextFireMissile = 0.0f;    // 子彈下一次能射擊的時間

    //連鎖閃電
    public GameObject Thunder;
    private bool created = false;            //生成閃電後改為true避免連續生成 
    GameObject cloneThunder;                 //生成閃電同時將此變數值指定為該閃電 用於之後刪除

    // 武器選擇框
    public Image nowWeaponImage;
    public Image nextWeaponImage;

    // 武器圖樣陣列
    public Sprite[] weaponSprites = new Sprite[2];
    
    
    // 武器類別選擇
    enum Weapon
    {
        basic,
        GatlingGun,
        Laser,
        Missile,
        Thunder,
        NumberOfWeapon

    }
    Weapon weapon = new Weapon();
    int weaponNumber = (int)Weapon.NumberOfWeapon; // 取得列舉的大小

    // 自訂類別的參考還是呼叫還是存取 還是?
    public Boundary boundary;

	// Use this for initialization
	void Start () {

        // 取得元件
        playerRigidbody = GetComponent<Rigidbody>();
        playerAudioSource = GetComponent<AudioSource>();

        // 設定初始武器
        weapon = Weapon.basic;
        //weapon = Weapon.GatlingGun;

    }


    void Update()
    {
        // 偵測是否切換武器
        SwitchWeapon();

        switch (weapon)
        {
            case Weapon.basic:
                {
                    // 如果按下了Fire1鍵 而且現在的時間已經超過可以發射的時間
                    if (Input.GetButton("Fire1") && Time.time >= nextFire)
                    {
                        // 下一次可以發射的時間往後延一個fireRate
                        nextFire = Time.time + fireRate;

                        // 建立子彈物件
                        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);

                        // 播放在玩家物件身上的audio source 也就是槍聲
                        playerAudioSource.Play();
                    }

                    // 切換武器欄位

                }
                break;
            case Weapon.GatlingGun:
                {
                    // 如果按下了Fire1鍵 而且現在的時間已經超過可以發射的時間
                    if (Input.GetButton("Fire1") && Time.time >= nextFireGatlin)
                    {
                        // 隨機產生一個子彈發射位置 模擬機槍
                        shotSpawnGatlin = new Vector3(Random.Range(shotSpawn.position.x - 0.5f, shotSpawn.position.x + 0.5f), shotSpawn.position.y, shotSpawn.position.z);

                        // 下一次可以發射的時間往後延一個fireRate
                        nextFireGatlin = Time.time + fireRateGatlin;

                        // 建立子彈物件
                        Instantiate(shot,shotSpawnGatlin , shotSpawn.rotation);

                        // 播放在玩家物件身上的audio source 也就是槍聲
                        playerAudioSource.Play();
                    }
                }
                break;
            case Weapon.Laser:
                {


                    if (Input.GetButton("Fire1"))
                    {
                        Instantiate(laser, shotSpawn.position, shotSpawn.rotation);
                    }

                    break;
                }
            case Weapon.Missile:
                {
                    {
                        // 如果按下了Fire1鍵 而且現在的時間已經超過可以發射的時間
                        if (Input.GetButton("Fire1") && Time.time >= nextFireMissile)
                        {
                            // 下一次可以發射的時間往後延一個fireRate
                            nextFireMissile = Time.time + fireRateMissile;

                            // 建立子彈物件 
                            Instantiate(missile, shotSpawn.position + new Vector3(0,0,0), shotSpawn.rotation);

                            // 播放在玩家物件身上的audio source 也就是槍聲
                            playerAudioSource.Play();
                        }

                        // 切換武器欄位

                    }
                    break;
                }

            case Weapon.Thunder:
                {
                    if (Input.GetButton("Fire1") )
                    {
                        
                        if (created == false)
                        {
                            cloneThunder = Instantiate(Thunder, shotSpawn.position, shotSpawn.rotation);
                            created = true;
                        }
                        
                    }
                    else if(Input.GetButtonUp("Fire1") || this.gameObject == null){
                        created = false;
                        if (cloneThunder != null)
                        {
                            Destroy(cloneThunder);
                        }
                        
                    }

                    if (cloneThunder != null) {
                        cloneThunder.transform.position = shotSpawn.position - new Vector3(0,0,0.5f);
                    }

                    break;
                }
            default:
                break;
        }
        // 如果武器選擇已經不是閃電
        if (weapon != Weapon.Thunder)
        {
            // 重置生成閃電的布林
            created = false;
            // 刪除閃電
            if (cloneThunder != null)
            {
                Destroy(cloneThunder);
            }
        }


    }


	// Update is called once per frame
	void FixedUpdate ()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");


        //角色的移動不再用addforce 避免因為沒有摩擦力的關係無法煞車 
        //playerRigidbody.AddForce(new Vector3(moveHorizontal, 0, moveVertical) * 10);

        // 宣告一個vector3變數 存入按鍵輸入的值
        Vector3 moveDirection = new Vector3(moveHorizontal, 0, moveVertical);

        // 這個方法會直接指定速度 所以input值為0的時候 玩家的速度就會直接為0 
        playerRigidbody.velocity = moveDirection * speed;

        // 限制玩家移動位置 而限制的值由最上面的自訂類別Boundary提供 所以寫boundary.xMin..
        playerRigidbody.position = new Vector3(
            Mathf.Clamp(playerRigidbody.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(playerRigidbody.position.z, boundary.zMin, boundary.zMax)
            );

        // 依照x軸的速度大小進行旋轉
        playerRigidbody.rotation = Quaternion.Euler(0, 0, playerRigidbody.velocity.x * -tilt );

	}

    // 切換武器方法
    public void SwitchWeapon()
    {
        // 如果按下了SwitchWeapon(目前設定為'E')
        if (Input.GetButtonDown("SwitchWeapon"))
        {
            // 一般情況下切換到下一個武器
            if ((int)weapon < weaponNumber - 1)
            {   
                weapon += 1;

                // 如果目前的武器已經是最後一把了 將下一把武器的圖樣設為第一把武器的圖樣
                if ((int)weapon == weaponNumber - 1) nextWeaponImage.sprite = weaponSprites[0];
                else nextWeaponImage.sprite = weaponSprites[(int)weapon + 1];


                // 目前武器的圖樣設為目前武器的圖樣
                nowWeaponImage.sprite = weaponSprites[(int)weapon];
                
            }
            // 如果目前武器已經是武器類別的最後一個有效武器 轉為切換到第一種武器
            else if ((int)weapon == weaponNumber - 1)
            {
                // 目前的武器設為第一把
                weapon = 0;

                // 下一把武器的圖樣設為第二把武器的圖樣
                nextWeaponImage.sprite = weaponSprites[1];

                // 目前武器的圖樣設為目前武器的圖樣
                nowWeaponImage.sprite = weaponSprites[(int)weapon];
            }
        }
    }

    // 尋找最近的敵人(物件)方法
   public Transform GetClosestEnemy(Transform[] enemies)
    {
        // 儲存最近的敵人位置
        Transform bestTarget = null;

        // 儲存最近敵人的距離長度 
        float closestDistanceSqr = Mathf.Infinity;

        // 計算距離的起始點 這裡是玩家
        Vector3 currentPosition = transform.position;

        // 逐一計算敵人陣列的每個內容
        foreach (Transform potentialTarget in enemies)
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
