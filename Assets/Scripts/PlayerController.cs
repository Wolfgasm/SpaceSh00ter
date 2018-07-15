using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    
    // 子彈生成位置
    public GameObject shot;
    public Transform shotSpawn;
    // 機槍子彈生成位置
    Vector3 shotSpawnGatlin;

    // 基本武器子彈射擊速度
    public float fireRate;
    // 加特林機槍子彈射擊速度
    public float fireRateGatlin;
    
    // 基礎武器下一次能射擊的時間
    private float nextFire = 0.0f;
    // 機槍下一次能射擊的時間
    private float nextFireGatlin = 0.0f;

    // 武器類別選擇
    enum Weapon
    {
        basic,
        GatlingGun,
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
        // 切換武器方法
        if (Input.GetButtonDown("SwitchWeapon"))
        {
            if ((int)weapon < weaponNumber - 1)
            {
                weapon += 1;
            }
            else if((int)weapon == weaponNumber -1)
            {
                weapon = 0;
            }
        }

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
            default:
                break;
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
            }
            // 如果目前武器已經是武器類別的最後一個有效武器 轉為切換到第一種武器
            else if ((int)weapon == weaponNumber - 1)
            {
                weapon = 0;
            }
        }
    }
}
