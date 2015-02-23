using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPC_Behaviour : MonoBehaviour {

    [System.Serializable]
    public class Waypoints
    {
        public Transform[] LeftRotationWP;
        public Transform[] RightRotationWP;
        public Transform[] UpRotationWP;
        public Transform[] DownRotationWP;
    }

    public Waypoints NPCWaypoints = new Waypoints();
    List<Transform> CurrentWaypoints = new List<Transform>();    

    [SerializeField] bool WaypointsAssigned;
    [SerializeField] float Speed;
    [SerializeField] string CurrentRotation;   
    [SerializeField] int CurrentWaypoint = 0;

    private PlatformRotationController PRC_Script;  
    [SerializeField] bool NPC_Rotating;   

	// Use this for initialization
	void Start () {

        DetectRotation();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        if (PRC_Script != null)
        {
            if (PRC_Script.bl_RotatePlatform)
            {
                rigidbody.velocity = Vector3.zero;
                CurrentWaypoint = 0;
                NPC_Rotating = true;
                WaypointsAssigned = false;
            }
            if (!PRC_Script.bl_RotatePlatform && !WaypointsAssigned)
            {
                NPC_Rotating = false;
                DetectRotation();
            }
        }
        
        if (WaypointsAssigned && !NPC_Rotating)
        {
            if (CurrentWaypoints.Count > 0)
            {
                MoveNPC();
            }
           
        }
    }

    void MoveNPC()
    {
        Vector3 NPC_Direction = (CurrentWaypoints[CurrentWaypoint].transform.position - transform.position).normalized * Speed;
        rigidbody.velocity = NPC_Direction * Time.deltaTime;

        DetermineNextWaypoint();
    }

    void DetermineNextWaypoint()
    {
        if (CurrentRotation == "Up" || CurrentRotation == "Down")
        {
            if (transform.position.x <= CurrentWaypoints[CurrentWaypoint].transform.position.x + 0.2F && transform.position.x >= CurrentWaypoints[CurrentWaypoint].transform.position.x - 0.2F)
            {
                IncrementWaypoint();
            }           
        }

        if (CurrentRotation == "Left" || CurrentRotation == "Right")
        {

            if (transform.position.y <= CurrentWaypoints[CurrentWaypoint].transform.position.y + 0.2F && transform.position.y >= CurrentWaypoints[CurrentWaypoint].transform.position.y - 0.2F)
            {
                IncrementWaypoint();
            }         
        }
    }

    void IncrementWaypoint()
    {
        if (CurrentWaypoint == CurrentWaypoints.Count -1)
        {
            CurrentWaypoint = 0;
        }
        else
        {
            CurrentWaypoint++;
        }
    }

    void DetectRotation()
    {
        int PlayerRotation = Mathf.RoundToInt(transform.eulerAngles.z);        

        if (PlayerRotation == 0)
        {
            CurrentRotation = "Up";
        }
        if (PlayerRotation == 90)
        {
            CurrentRotation = "Left";
        }
        if (PlayerRotation == 180)
        {
            CurrentRotation = "Down";
        }
        if (PlayerRotation == 270)
        {
            CurrentRotation = "Right";
        }

        AssignWaypoints();
    }

    void AssignWaypoints()
    {
        CurrentWaypoints.Clear();

        if (CurrentRotation == "Up")
        {
            for (int i = 0; i < NPCWaypoints.UpRotationWP.Length; i++)
            {
                CurrentWaypoints.Add(NPCWaypoints.UpRotationWP[i]);
                
            }
        }
        if (CurrentRotation == "Left")
        {
            for (int i = 0; i < NPCWaypoints.LeftRotationWP.Length; i++)
            {
                CurrentWaypoints.Add(NPCWaypoints.LeftRotationWP[i]);
            }
        }
        if (CurrentRotation == "Down")
        {
            for (int i = 0; i < NPCWaypoints.DownRotationWP.Length; i++)
            {
                CurrentWaypoints.Add(NPCWaypoints.DownRotationWP[i]);
            }
        }
        if (CurrentRotation == "Right")
        {
            for (int i = 0; i < NPCWaypoints.RightRotationWP.Length; i++)
            {
                CurrentWaypoints.Add(NPCWaypoints.RightRotationWP[i]);
            }
        }

        WaypointsAssigned = true;
    }

    void OnTriggerEnter(Collider cc_Hit)
    {
        if (cc_Hit.gameObject.name == "RotationZone")
        {
            transform.parent = cc_Hit.gameObject.transform.root;
            PRC_Script = cc_Hit.transform.root.GetComponent<PlatformRotationController>();
        }
        if (cc_Hit.gameObject.tag == "Platform")
        {
            transform.parent = null;
            PRC_Script = null;
        }
    }

    void OnTriggerExit(Collider cc_Hit)
    {
        if (cc_Hit.gameObject.name == "RotationZone")
        {
            transform.parent = null;
            PRC_Script = null;
        }
    }
}
