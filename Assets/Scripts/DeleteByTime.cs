using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteByTime : MonoBehaviour {

    public float lifeTime;

	// Use this for initialization
	void Start () {
        // 在幾秒後刪除此物件
        Destroy(gameObject,lifeTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
