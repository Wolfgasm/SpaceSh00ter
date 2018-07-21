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

// 基礎武器類別
[System.Serializable]
public class BasicWeapon
{
    
    // 基礎武器屬性
    public GameObject shot;         // 子彈物體
    public Transform shotSpawn;     // 子彈生成位置
    public float fireRate;          // 子彈射擊速度
    private float nextFire = 0.0f;  // 程式用來計算子彈下一次能射擊的時間

    // nextFire的讀寫屬性
    public float NextFire { get; set; }


}

// 機槍武器類別
[System.Serializable]
public class GatlinWeapon
{
    public float fireRateGatlin;            // 子彈射擊速度
    private float nextFireGatlin = 0.0f;    // 子彈下一次能射擊的時間
    // nextFireGatlin的讀寫屬性
    public float NextFireGatlin { get; set; }


}

// 雷射武器類別
[System.Serializable]
public class LaserWeapon
{
    // 雷射武器屬性
    public GameObject laser;            // 要生成的雷射物件
    private bool laserCreated = false;  // 是否已經生成雷射物件
    private GameObject cloneLaser;      // 儲存生成的雷射物件複製品
    private LineRenderer laserLine;     // 雷射物件的LineRenderer
    public GameObject explosionOfLaser; // 雷射造成的特效
    public GameObject explosionOfLaser02;
    public float laserInterval;         // 雷射輸出頻率
    private float nextLaserInterval;    // 控制雷射輸出頻率用的變數
    public int damage;


    public bool LaserCreated { get; set; }

    public GameObject CloneLaser { get; set; }

    public LineRenderer LaserLine { get; set; }

    public float NextLaserInterval { get; set; }




}

// 火箭武器類別
[System.Serializable]
public class MissileWeapon
{
    public GameObject missile;
    public float fireRateMissile;            // 子彈射擊速度
    private float nextFireMissile = 0.0f;    // 子彈下一次能射擊的時間
    public float NextFireMissile { get; set; }

}

// 閃電武器類別
[System.Serializable]
public class ThunderWeapon
{
    public GameObject Thunder;
    private bool thunderCreated = false;            //生成閃電後改為true避免連續生成 
    private GameObject cloneThunder;                 //生成閃電同時將此變數值指定為該閃電 用於之後刪除

    public bool ThunderCreated { get; set; }
    public GameObject CloneThunder { get; set; }

}

public class PlayerController : MonoBehaviour {

    // 自訂類別的參考還是呼叫還是存取 還是?
    public Boundary boundary;
    public BasicWeapon basicWeapon;
    public GatlinWeapon gatlinWeapon;
    public LaserWeapon laserWeapon;
    public MissileWeapon missileWeapon;
    public ThunderWeapon thunderWeapon;
    
    // 玩家的rigidbody
    Rigidbody playerRigidbody;

    // 玩家的destroyable
    DestroyAble playerDestroyAble;
    
    // 玩家的Audio Source
    AudioSource playerAudioSource;      // 第一個AS元件 用來撥NyanCat音樂
    AudioSource playerAudioSource2;     // 第二個AS元件 用來處理槍聲
    AudioSource[] playerAudioSourceS;   // 儲存所有AudioSource元件

    //
    DestroyAble destroyable;

    // 移動速度
    public float speed;

    // 旋轉角度
    public float tilt;

    // 武器選擇框
    public Image nowWeaponImage;
    public Image nextWeaponImage;

    // 武器圖樣陣列
    public Sprite[] weaponSprites = new Sprite[2];
    
    
    // 武器類別列舉
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



	// Use this for initialization
	void Start () {

        // 取得元件
        playerRigidbody = GetComponent<Rigidbody>();
        playerDestroyAble = GetComponent<DestroyAble>();

        // 取得AudioSource元件 因為有兩個所以用陣列
        playerAudioSourceS = GetComponents<AudioSource>();
        playerAudioSource = playerAudioSourceS[0];
        playerAudioSource2 = playerAudioSourceS[1];
        

        // 設定初始武器
        weapon = Weapon.basic;


    }


    void Update()
    {
        // 偵測是否切換武器
        SwitchWeapon();




        // 武器種類
        switch (weapon)
        {
            case Weapon.basic:
                {
                    // 如果按下了Fire1鍵 而且現在的時間已經超過可以發射的時間
                    if (Input.GetButton("Fire1") && Time.time >= basicWeapon.NextFire)
                    {
                        // 下一次可以發射的時間往後延一個fireRate
                        basicWeapon.NextFire = Time.time + basicWeapon.fireRate;

                        // 建立子彈物件
                        Instantiate(basicWeapon.shot, basicWeapon.shotSpawn.position, basicWeapon.shotSpawn.rotation);

                        // 播放槍聲
                        playerAudioSource2.Play();
                    }

                    // 切換武器欄位

                }
                break;
            case Weapon.GatlingGun:
                {
                    // 如果按下了Fire1鍵 而且現在的時間已經超過可以發射的時間
                    if (Input.GetButton("Fire1") && Time.time >= gatlinWeapon.NextFireGatlin)
                    {
                        // 隨機產生一個子彈發射位置 模擬機槍
                        Vector3 shotSpawnGatlin = new Vector3(Random.Range(basicWeapon.shotSpawn.position.x - 0.5f, basicWeapon.shotSpawn.position.x + 0.5f), basicWeapon.shotSpawn.position.y, basicWeapon.shotSpawn.position.z);

                        // 下一次可以發射的時間往後延一個fireRate
                        gatlinWeapon.NextFireGatlin = Time.time + gatlinWeapon.fireRateGatlin;

                        // 建立子彈物件
                        Instantiate(basicWeapon.shot, shotSpawnGatlin , basicWeapon.shotSpawn.rotation);

                        // 播放槍聲
                        playerAudioSource2.Play();
                    }
                }
                break;
            case Weapon.Laser:
                {
                    if (Input.GetButton("Fire1"))
                    {
                       

                        // 用laserCreated變數避免重複生成雷射
                        if (laserWeapon.LaserCreated == false)
                        {
                            

                            // 生成雷射
                            laserWeapon.CloneLaser = Instantiate(laserWeapon.laser, basicWeapon.shotSpawn.position, basicWeapon.shotSpawn.rotation);

                            // 取得子物件的Linerenderer
                            laserWeapon.LaserLine = laserWeapon.CloneLaser.GetComponentInChildren<LineRenderer>();

                            // 重置設線起始寬度
                            laserWeapon.LaserLine.startWidth = 0;
                            

                            // 如果還沒有撥過這個音樂
                            if (playerAudioSource.isPlaying == false)
                            {
                                // 撥放nyanCat音樂
                                playerAudioSource.Play();
                            }
                            else // 如果已經撥過
                            {
                                // 取消暫停
                                playerAudioSource.UnPause();
                            }

                            // 避免重複執行
                            laserWeapon.LaserCreated = true;
                        }

                        // 讓射線寬度逐漸增加
                        if (laserWeapon.LaserLine.startWidth < 0.3)
                        {
                            laserWeapon.LaserLine.startWidth += 0.005f;
                            
                        }
                        else if (laserWeapon.LaserLine.startWidth < 1.2f)
                        {
                            laserWeapon.LaserLine.startWidth += 0.16f;
                            
                        }

                        // 用來儲存射線碰到的物體的資訊
                        RaycastHit myRaycastHit;
                        

                        // 利用設線偵測是否擊中敵人 
                        // 如果有擊中
                        if (Physics.Raycast(transform.position, transform.TransformDirection(transform.forward), out myRaycastHit))
                        {
                            
                            // 限制雷射傷害速度
                            if (laserWeapon.NextLaserInterval <= Time.time)
                            {
                                destroyable = myRaycastHit.collider.gameObject.GetComponent<DestroyAble>();
                                destroyable.health -= laserWeapon.damage;

                                laserWeapon.NextLaserInterval = Time.time + laserWeapon.laserInterval;

                                // 生成擊中特效
                                Instantiate(laserWeapon.explosionOfLaser, new Vector3(transform.position.x, -6, myRaycastHit.transform.position.z), laserWeapon.explosionOfLaser.transform.rotation);
                                Instantiate(laserWeapon.explosionOfLaser02, new Vector3(transform.position.x, 0, myRaycastHit.transform.position.z), laserWeapon.explosionOfLaser02.transform.rotation);

                                
                                
                            }

                            // 將雷射尾端設為設線所碰到的物體
                            laserWeapon.LaserLine.SetPosition(1, new Vector3(0, 0, myRaycastHit.transform.position.z + 2.5f));

                        }
                        // 如果沒有擊中
                        else {
                            // 射線長度設為固定值
                            laserWeapon.LaserLine.SetPosition(1, new Vector3(0, 0, 100));

                        }

                    }

                    // 如果不再按壓發射鍵 或者玩家已經消失
                    else if (Input.GetButtonUp("Fire1"))
                    {
                        // 重置射線的生成狀態
                        laserWeapon.LaserCreated = false;

                        // 刪除剛才建立的射線物件
                        if (laserWeapon.CloneLaser != null)
                        {
                            Destroy(laserWeapon.CloneLaser);
                        }

                        // 暫停nyanCat音樂
                        playerAudioSource.Pause();
                    }

                    // 如果射線的母物件存在 將他的座標設定為玩家的槍口位置
                    if (laserWeapon.CloneLaser != null)
                    {
                        laserWeapon.CloneLaser.transform.position = basicWeapon.shotSpawn.position - new Vector3(0, 0, 0.5f);
                    }


                    break;
                }
            case Weapon.Missile:
                {
                    {
                        // 如果按下了Fire1鍵 而且現在的時間已經超過可以發射的時間
                        if (Input.GetButton("Fire1") && Time.time >= missileWeapon.NextFireMissile)
                        {
                            // 下一次可以發射的時間往後延一個fireRate
                            missileWeapon.NextFireMissile = Time.time + missileWeapon.fireRateMissile;

                            // 建立子彈物件 
                            Instantiate(missileWeapon.missile, basicWeapon.shotSpawn.position + new Vector3(0,0,0), basicWeapon.shotSpawn.rotation);

                            // 播放在玩家物件身上的audio source 也就是槍聲
                            playerAudioSource2.Play();
                        }

                        // 切換武器欄位

                    }
                    break;
                }

            case Weapon.Thunder:
                {
                    if (Input.GetButton("Fire1") )
                    {
                        
                        if (thunderWeapon.ThunderCreated == false)
                        {
                            thunderWeapon.CloneThunder = Instantiate(thunderWeapon.Thunder, basicWeapon.shotSpawn.position, basicWeapon.shotSpawn.rotation);
                            thunderWeapon.ThunderCreated = true;
                        }
                        
                    }
                    else if(Input.GetButtonUp("Fire1") || this.gameObject == null){
                        thunderWeapon.ThunderCreated = false;
                        if (thunderWeapon.CloneThunder != null)
                        {
                            Destroy(thunderWeapon.CloneThunder);
                        }
                        
                    }

                    if (thunderWeapon.CloneThunder != null) {
                        thunderWeapon.CloneThunder.transform.position = basicWeapon.shotSpawn.position - new Vector3(0,0,0.5f);
                    }

                    break;
                }
            default:
                break;
        }
        // 如果武器選擇已經不是閃電
        if (weapon != Weapon.Thunder || playerDestroyAble.health <=0)
        {
            // 重置生成閃電的布林
            thunderWeapon.ThunderCreated = false;
            // 刪除閃電
            if (thunderWeapon.CloneThunder != null)
            {
                Destroy(thunderWeapon.CloneThunder);
            }
        }
        // 如果武器選擇已經不是雷射
        if (weapon != Weapon.Laser || playerDestroyAble.health <= 0)
        {
            laserWeapon.LaserCreated = false;

            if (laserWeapon.CloneLaser != null)
            {
                Destroy(laserWeapon.CloneLaser);

            }
            // 暫停nyanCat音樂
            playerAudioSource.Stop();

        }


    }


	// 玩家動作
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
