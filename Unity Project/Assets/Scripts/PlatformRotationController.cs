using UnityEngine;
using System.Collections;

public class PlatformRotationController : MonoBehaviour {

    public float fl_Speed;
    public float fl_RotationDegreesAmount;
    public float fl_RotationAddedOn;
    private float fl_TotalRotation;
    public bool bl_RotatePlatform;
    private Quaternion PlatformRotation;
    public bool bl_FinishedRotation;

    

	// Use this for initialization
	void Start () {
       
        bl_RotatePlatform = false;
	
	}
	
	// Update is called once per frame
	void Update () {        

        if (Mathf.Abs(fl_TotalRotation) < Mathf.Abs(fl_RotationDegreesAmount) && bl_RotatePlatform)
        {
            PlatformRotate();
            bl_FinishedRotation = false;
        }

        if (Mathf.Abs(fl_TotalRotation) > Mathf.Abs(fl_RotationDegreesAmount) && bl_RotatePlatform)
        {
			transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, fl_RotationDegreesAmount);
            bl_RotatePlatform = false;
            bl_FinishedRotation = true;
        }
	
	}

    void PlatformRotate()
    {
        float _fl_currentAngle = transform.rotation.eulerAngles.z;
        transform.rotation = Quaternion.AngleAxis(_fl_currentAngle + (Time.deltaTime * fl_Speed), Vector3.forward);
        fl_TotalRotation += Time.deltaTime * fl_Speed;
    }
}
