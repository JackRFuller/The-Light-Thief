using UnityEngine;
using System.Collections;

public class SwitchManager : MonoBehaviour {

    public GameObject Platform;
    public PlatformRotationController PRC_Script;

	// Use this for initialization
	void Start () {

        PRC_Script = Platform.GetComponent<PlatformRotationController>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
