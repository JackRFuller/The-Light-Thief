using UnityEngine;
using System.Collections;

public class MovementManager : MonoBehaviour {

    //PlayerVariables
    [SerializeField] GameObject PC;
    [SerializeField] bool PlayerMoving;
    public float PlayerSpeed;
    private Vector3 DesiredVelocity;
    private float lastSqrMag;
    private string PlayerMoveRotation;

    //WaypointVariables;
    public GameObject Waypoint;
    [SerializeField] Vector3 ClickPosition;
    [SerializeField] Vector3 WaypointPosition;
    [SerializeField] bool WaypointPlaced;
    [SerializeField] GameObject CurrentWaypoint;

	// Use this for initialization
	void Start () {

        PC = GameObject.FindGameObjectWithTag("Player");
        WaypointPlaced = false;
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonUp(0))
        {
            DetermineWaypoint();	
        }

       
	}

    void FixedUpdate()
    {
        if (PlayerMoving)
        {
            Vector3 PlayerDirection = (CurrentWaypoint.transform.position - PC.transform.position).normalized * PlayerSpeed;
            PC.rigidbody.velocity =  PlayerDirection * Time.deltaTime;

            DeterminePlayerMovePosition();
        }
    }

    void DeterminePlayerMovePosition()
    {
        if (PlayerMoveRotation == "Up")
        {            
            if (PC.transform.position.x <= CurrentWaypoint.transform.position.x + 0.2 && PC.transform.position.x >= CurrentWaypoint.transform.position.x - 0.2)
            {
                PlayerMoving = false;
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
            if (hit.collider.tag == "Platform")
            {
               
                ClickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                CheckPlayerRotation();
            }
        }       
    }

    void CheckPlayerRotation()
    {
        string PlayerRotation = null;

        if (PC.transform.eulerAngles.z == 0)
        {
            PlayerRotation = "Up";
        }

        PlayerMoveRotation = PlayerRotation;
        CheckPlayerPosition(PlayerRotation);
    }

    void CheckPlayerPosition(string PlayerRot)
    {
        if (PlayerRot == "Up")
        {
            
            if (PC.transform.position.y == ClickPosition.y || (PC.transform.position.y <= ClickPosition.y + 2 && PC.transform.position.y > ClickPosition.y - 2))
            {
                WaypointPosition = new Vector3(ClickPosition.x, PC.transform.position.y, 20);
                PlaceWaypoint();
            }
        }
    }

    void PlaceWaypoint()
    {
        if (WaypointPlaced)
        {
            CurrentWaypoint.transform.position = WaypointPosition;
        }

        if (!WaypointPlaced)
        {
            Instantiate(Waypoint, WaypointPosition, Waypoint.transform.rotation);
            CurrentWaypoint = GameObject.Find("Waypoint(Clone)");
            WaypointPlaced = true;
        }

        SetPlayerMovement();
    }

    void SetPlayerMovement()
    {

        PlayerMoving = true;
    }
}
