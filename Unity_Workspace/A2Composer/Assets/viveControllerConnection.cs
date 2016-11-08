using UnityEngine;
using System.Collections;

public class viveControllerConnection : MonoBehaviour {
    private SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device device;
    public setupScene SetupScene;
    // Use this for initialization
    void Start () {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
        //setupScene = GameObject.Find("setupScene").GetComponent<SteamVR_TrackedObject>();
        Debug.Log("init " ,trackedObject);
        
	}
	
	// Update is called once per frame
	void Update () {
        device = SteamVR_Controller.Input((int)trackedObject.index);
        //Debug.Log(device);
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("PRESSED");
            device.TriggerHapticPulse(3000);

            device.TriggerHapticPulse(3000);
            SetupScene.calibrationDone();
        }
	}
}
