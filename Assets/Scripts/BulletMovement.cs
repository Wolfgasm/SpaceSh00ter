using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour {

    Rigidbody bulletRigibody;

    public float speed;
	// Use this for initialization
	void Start () {
        bulletRigibody = GetComponent<Rigidbody>();

        // 剛生成時就有一個向z軸正向的動量
        //bulletRigibody.velocity = new Vector3(0, 0, 1 * speed);

        // 這行跟上面一樣
        bulletRigibody.velocity = transform.forward * speed;
	}
	

}
