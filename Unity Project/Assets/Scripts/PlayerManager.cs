using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    private MovementManager MM_Script;
    private GameObject PC_Mesh;
    private Animator PC_Animation;
    private Vector3 LocalPosition;

	// Use this for initialization
	void Start () {

        MM_Script = GameObject.Find("Main Camera").GetComponent<MovementManager>();
        PC_Mesh = GameObject.FindGameObjectWithTag("PC_Mesh");
        LocalPosition = PC_Mesh.transform.localPosition;
        PC_Animation = GameObject.FindGameObjectWithTag("PC_Mesh").GetComponent<Animator>();
	
	}
	
	
	

    public void TurnOnAnimations()
    {
        PC_Mesh.transform.localPosition = LocalPosition;  
     
        if (MM_Script.PlayerSpeed == MM_Script.PlayerStealthSpeed)
        {
            PC_Animation.SetBool("Walk", true);             
            PC_Animation.SetBool("Idle", false);
            PC_Animation.SetBool("Run", false);
            
        }

        if (MM_Script.PlayerSpeed == MM_Script.PlayerRunningSpeed)
        {
            PC_Animation.SetBool("Run", true);
            PC_Animation.SetBool("Walk", false);
            PC_Animation.SetBool("Idle", false);
          
        } 
    }

    public void TurnOffAnimations()
    {
        PC_Mesh.transform.localPosition = LocalPosition;
        PC_Animation.SetBool("Run", false);
        PC_Animation.SetBool("Walk", false);
        PC_Animation.SetBool("Idle", true);
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
