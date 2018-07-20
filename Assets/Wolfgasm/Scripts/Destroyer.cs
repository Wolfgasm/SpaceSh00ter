using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour {

    public int damage;

    public GameObject hitEffect;

    private Vector3 prevPos;
    private Vector3 incomingVec;
    private Vector3 reflectVec;
    private RaycastHit[] rayCastHits;
    private RaycastHit myRaycastHit;


    public Vector3 IncomingVec { get; set; }
   // public Vector3 ReflectVec { get; set; }
   // public RaycastHit[] RayCastHits { get; set; }
    public RaycastHit MyRaycastHit {
        get
        {
            return myRaycastHit;
        }
        

    }

    public RaycastHit[] RaycastHits
    {
        get {
            return rayCastHits;
        }
        set {
            rayCastHits = value;
        }

    }

    public Vector3 ReflectVec
    {
        get {
            return reflectVec;
        }
    }

    // Use this for initialization
    void Start () {
        prevPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        prevPos = transform.position;
        
        // || 起點    ||                    方向                   ||    長度( magnutude會回傳向量的長度) ||   
        rayCastHits  = Physics.RaycastAll(new Ray(prevPos, (transform.position - prevPos).normalized), (transform.position - prevPos).magnitude);

        for (int i = 0; i < rayCastHits.Length; i++)
        {

            if (true)
            {
                incomingVec = rayCastHits[i].point - rayCastHits[i].transform.position;
                reflectVec = Vector3.Reflect(incomingVec, rayCastHits[i].normal);
                myRaycastHit = rayCastHits[i];
                Debug.Log("nigger");
            }
        }
        
        /*
        if (Physics.Raycast(prevPos, ((transform.position - prevPos).normalized), out myRaycastHit, ((transform.position - prevPos).magnitude)))
        {
            if (myRaycastHit.collider.gameObject.tag == "Enemies")
            {
                incomingVec = myRaycastHit.point - myRaycastHit.transform.position;
                reflectVec = Vector3.Reflect(incomingVec, myRaycastHit.normal);
                Debug.Log("Niggre");
            }
        }*/


	}
}
