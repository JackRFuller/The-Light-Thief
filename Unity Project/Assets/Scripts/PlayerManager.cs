using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    private MovementManager MM_Script;

	// Use this for initialization
	void Start () {

        MM_Script = GameObject.Find("Main Camera").GetComponent<MovementManager>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider cc_Hit)
    {
        if (cc_Hit.gameObject.name == "RotationZone" && cc_Hit.gameObject.tag != "Platform")
        {
            transform.parent = cc_Hit.gameObject.transform.parent;
            MM_Script.DetermineRotatingPlatform();
        }
        if (cc_Hit.gameObject.name != "RotationZone" && cc_Hit.gameObject.tag == "Platform")
        {
            transform.parent = null;
            MM_Script.PRC_Script = null;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "RotatingPlatform" || col.gameObject.name == "Platform")
        {
            MM_Script.PlayerMoving = false;
            rigidbody.velocity = Vector3.zero;
        }
        
    }
}
