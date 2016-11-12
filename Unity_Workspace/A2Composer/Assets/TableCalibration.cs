using UnityEngine;
using System.Collections;

public class TableCalibration : MonoBehaviour {

    public Vector3[] positions;
    public bool[] setPositions;
    private GameObject m200;
    private GameObject m400;
    private GameObject m600;
    private GameObject m800;
    public setupScene setupScene;

    // Use this for initialization
    void Start () {
        positions = new Vector3[4];
        setPositions = new bool[4] { false, false, false, false };
        m200 = GameObject.Find("OvrvisionTracker200");
        m200.GetComponentInChildren<Renderer>().material.color = new Color(128, 0, 0);
        m400 = GameObject.Find("OvrvisionTracker400");
        m400.GetComponentInChildren<Renderer>().material.color = new Color(128, 0, 0);
        m600 = GameObject.Find("OvrvisionTracker600");
        m600.GetComponentInChildren<Renderer>().material.color = new Color(128, 0, 0);
        m800 = GameObject.Find("OvrvisionTracker800");
        m800.GetComponentInChildren<Renderer>().material.color = new Color(128, 0, 0);
    }

    private bool isMarkerVisible(Vector3 markerPos){
        if (markerPos.x < -1000.0f || markerPos.x > 1000.0f || markerPos.y < -1000.0f || markerPos.y > 1000.0f || markerPos.z < -1000.0f || markerPos.z > 1000.0f)
            return false;
        return true;
    }

    public void attemptCalibration() {
        if (isMarkerVisible(m200.transform.position)){
            positions[0] = m200.transform.position;
            setPositions[0] = true;
            m200.GetComponentInChildren<Renderer>().material.color = new Color(0, 128, 0);
            Debug.Log("Marker 200 calibrated successfully.");
        }
        if (isMarkerVisible(m400.transform.position)){
            positions[1] = m400.transform.position;
            setPositions[1] = true;
            m400.GetComponentInChildren<Renderer>().material.color = new Color(0, 128, 0);
            Debug.Log("Marker 400 calibrated successfully.");
        }
        if (isMarkerVisible(m600.transform.position)){
            positions[2] = m600.transform.position;
            setPositions[2] = true;
            m600.GetComponentInChildren<Renderer>().material.color = new Color(0, 128, 0);
            Debug.Log("Marker 600 calibrated successfully.");
        }
        if (isMarkerVisible(m800.transform.position)){
            positions[3] = m800.transform.position;
            setPositions[3] = true;
            m800.GetComponentInChildren<Renderer>().material.color = new Color(0, 128, 0);
            Debug.Log("Marker 800 calibrated successfully.");
        }
    }

    // Update is called once per frame
    void Update () {
        if (setPositions[0] && setPositions[1] && setPositions[2] && setPositions[3]){
            Debug.Log("Calibration successful.");
            setupScene.calibrationDone(positions);
            m200.SetActive(false);
            m400.SetActive(false);
            m600.SetActive(false);
            m800.SetActive(false);
            this.enabled = false;
        }
	}
}
