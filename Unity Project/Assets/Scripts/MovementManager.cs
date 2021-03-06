﻿using UnityEngine;
using System.Collections;

public class MovementManager : MonoBehaviour {

    //PlayerVariables
    [SerializeField] GameObject PC;
    [SerializeField] GameObject PC_Mesh;
    private PlayerManager PM_Script;
    private Vector3 PCMesh_Position;
    public bool PlayerMoving;
    public float PlayerSpeed;
    public float PlayerRunningSpeed;
    public float PlayerStealthSpeed;

    private Vector3 DesiredVelocity;
    private float lastSqrMag;
    public string PlayerMoveRotation;

    //WaypointVariables;
    public GameObject Waypoint;
    [SerializeField] Vector3 ClickPosition;
    [SerializeField] Vector3 WaypointPosition;
    [SerializeField] bool WaypointPlaced;
    [SerializeField] GameObject CurrentWaypoint;

    //Rotation
    public PlatformRotationController PRC_Script;

    //DoubleClickVariables
   
    private bool TimerRunning;
    private float TimeForDoubleClick = 0;
    public float Delay;

	// Use this for initialization
	void Start () {

        PC = GameObject.FindGameObjectWithTag("Player");
        PM_Script = PC.GetComponent<PlayerManager>();
        PC_Mesh = GameObject.FindGameObjectWithTag("PC_Mesh");
        PCMesh_Position = PC_Mesh.transform.localPosition;
        WaypointPlaced = false;
        DetermineOrientation();
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey("r"))
        {
            Application.LoadLevel(Application.loadedLevelName);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (Time.time - TimeForDoubleClick < Delay)
            {
                //Debug.Log("DoubleClick");
                PlayerSpeed = PlayerRunningSpeed;
            }
            else
            {
                PlayerSpeed = PlayerStealthSpeed;
            }

            TimeForDoubleClick = Time.time;
            DetermineWaypoint();
            DetermineSwitch();	        
        }

        

	}
    //PlayerRotation-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void DetermineSwitch()
    {
        Ray SwitchRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(SwitchRay, out hit, 100))
        {
            if (hit.collider.tag == "Switch")
            {
                PRC_Script = hit.collider.gameObject.GetComponent<SwitchManager>().PRC_Script;
                PRC_Script.fl_RotationDegreesAmount += PRC_Script.fl_RotationAddedOn;
                PRC_Script.bl_RotatePlatform = true;
            }
        }      
    }

    void DetermineOrientation()
    {
        int PlayerRotation = Mathf.RoundToInt(PC.transform.eulerAngles.z);        

        if (PlayerRotation == 90)
        {
            PlayerMoveRotation = ("Left");
        }
        if (PlayerRotation == 180)
        {
            PlayerMoveRotation = ("Down");
        }
        if (PlayerRotation == 270)
        {
            PlayerMoveRotation = ("Right");
        }
        if (PlayerRotation == 0)
        {
            PlayerMoveRotation = ("Up");
        }

         PC.transform.eulerAngles = new Vector3(0, 0, PlayerRotation);

        
    }


    public void DetermineRotatingPlatform()
    {
        //PRC_Script = PC.transform.parent.GetComponent<PlatformRotationController>();
    }


    //PlayerMovement-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    void FixedUpdate()
    {

        PC_Mesh.transform.localPosition = PCMesh_Position;
        if (PlayerMoving)
        {
            PM_Script.TurnOnAnimations();
            Vector3 PlayerDirection = (CurrentWaypoint.transform.position - PC.transform.position).normalized * PlayerSpeed;
            PC.rigidbody.velocity =  PlayerDirection * Time.deltaTime;
            
            DeterminePlayerMovePosition();
        }
    }    

    void DeterminePlayerMovePosition()
    {
        if (PlayerMoveRotation == "Up" || PlayerMoveRotation == "Down")
        {
           
            if (PC.transform.position.x <= CurrentWaypoint.transform.position.x + 0.2 && PC.transform.position.x >= CurrentWaypoint.transform.position.x - 0.2)
            {
                PlayerMoving = false;
                PM_Script.TurnOffAnimations();
                PC.rigidbody.velocity = Vector3.zero;               
            }
        }

        if (PlayerMoveRotation == "Right" || PlayerMoveRotation == "Left")
        {
            if (PC.transform.position.y <= CurrentWaypoint.transform.position.y + 0.2 && PC.transform.position.y >= CurrentWaypoint.transform.position.y - 0.2)
            {
                PlayerMoving = false;
                PM_Script.TurnOffAnimations();
                PC.rigidbody.velocity = Vector3.zero;
            }
        }

       
    }

    

   //WaypointPlacement--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void DetermineWaypoint()
    {
        Ray WaypointRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(WaypointRay, out hit, 100))
        {
            if (hit.collider.tag == "Platform" || hit.collider.gameObject.name == "RotationZone")
            {               
                ClickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                DetermineOrientation();
                CheckPlayerRotation();
            }
        }       
    }

    void CheckPlayerRotation()
    {
        
        CheckPlayerPosition();
    }

    void CheckPlayerPosition()
    {

        if (PlayerMoveRotation == "Up" || PlayerMoveRotation == "Down")
        {            
            if (PC.transform.position.y == ClickPosition.y || (PC.transform.position.y <= ClickPosition.y + 2 && PC.transform.position.y > ClickPosition.y - 2))
            {
                WaypointPosition = new Vector3(ClickPosition.x, PC.transform.position.y, 20);
                PlaceWaypoint();
            }
        }
        if (PlayerMoveRotation == "Right" || PlayerMoveRotation == "Left")
        {
            if (PC.transform.position.x == ClickPosition.x || (PC.transform.position.x <= ClickPosition.x + 2 && PC.transform.position.x > ClickPosition.x - 2))
            {
                WaypointPosition = new Vector3(PC.transform.position.x, ClickPosition.y, 20);
                PlaceWaypoint();
            }
        }
    }

    void PlaceWaypoint()
    {
        if (WaypointPlaced)
        {
            CurrentWaypoint.transform.position = WaypointPosition;
            CurrentWaypoint.audio.Play();
            CurrentWaypoint.transform.FindChild("WaypointSymbol").gameObject.animation.Play("Stealth_Grow");
        }

        if (!WaypointPlaced)
        {
            Instantiate(Waypoint, WaypointPosition, Waypoint.transform.rotation);           
            CurrentWaypoint = GameObject.Find("Waypoint(Clone)");
            CurrentWaypoint.audio.Play();
            CurrentWaypoint.transform.FindChild("WaypointSymbol").gameObject.animation.Play("Stealth_Grow");
            WaypointPlaced = true;
        }

       
        //SetPlayerMovement();
        DrawRaycast();
    }

    void DrawRaycast()
    {
        bool NoCollision = true;
        Vector3 forward = WaypointPosition - PC.transform.position;
        RaycastHit[] hits;
        hits = Physics.RaycastAll(PC.transform.position, forward, 100F);
        int i = 0;
        while (i < hits.Length && NoCollision)
        {
            RaycastHit hit = hits[i];
            //Debug.Log(hit.collider.gameObject.name + i.ToString());
            if (hit.collider.gameObject.name == "RotatingPlatform" || hit.collider.gameObject.name == "Platform")
            {
                if (i == 0)
                {
                    //NoCollision = false;
                }
                
            }

            i++;
        }

        if (NoCollision)
        {
            SetPlayerMovement();
        }
        else
        {
            //Debug.Log("Collision");
        }
    }

    void SetPlayerMovement()
    {
        if (PlayerMoveRotation == "Up")
        {
            if (CurrentWaypoint.transform.position.x < PC.transform.position.x)
            {
                PC_Mesh.transform.localEulerAngles = new Vector3(0, 270, 0);
            }
            if (CurrentWaypoint.transform.position.x > PC.transform.position.x)
            {
                PC_Mesh.transform.localEulerAngles = new Vector3(0, 90, 0);
            }
        }

        if (PlayerMoveRotation == "Down")
        {
            if (CurrentWaypoint.transform.position.x > PC.transform.position.x)
            {
                PC_Mesh.transform.localEulerAngles = new Vector3(0, 270, 0);
            }
            if (CurrentWaypoint.transform.position.x < PC.transform.position.x)
            {
                PC_Mesh.transform.localEulerAngles = new Vector3(0, 90, 0);
            }
        }
        if (PlayerMoveRotation == "Left")
        {

            if (CurrentWaypoint.transform.position.y < PC.transform.position.y)
            {
                PC_Mesh.transform.localEulerAngles = new Vector3(0, 270, 0);
            }
            if (CurrentWaypoint.transform.position.y > PC.transform.position.y)
            {
                PC_Mesh.transform.localEulerAngles = new Vector3(0, 90, 0);
            }
        }

        if (PlayerMoveRotation == "Right")
        {

            if (CurrentWaypoint.transform.position.y > PC.transform.position.y)
            {
                PC_Mesh.transform.localEulerAngles = new Vector3(0, 270, 0);
            }
            if (CurrentWaypoint.transform.position.y < PC.transform.position.y)
            {
                PC_Mesh.transform.localEulerAngles = new Vector3(0, 90, 0);
            }
        }
        PlayerMoving = true;
       
    }
}
