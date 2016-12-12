using UnityEngine;
using System.Collections;

public class TableCalibration : MonoBehaviour {

    private Vector3[] positions;
    private Vector3 center;
    private bool[] setPositions;
    private GameObject m200;
    private GameObject m400;
    private GameObject m600;
    private GameObject m800;
    public setupScene setupScene;
    public float planeZOffset = -0.15f;

    public Vector3[] getPositions(){
        return positions;
    }

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
            positions[0].y += planeZOffset;
            setPositions[0] = true;
            m200.GetComponentInChildren<Renderer>().material.color = new Color(0, 128, 0);
            Debug.Log("Marker 200 calibrated successfully.");
        }
        if (isMarkerVisible(m400.transform.position)){
            positions[1] = m400.transform.position;
            positions[1].y += planeZOffset;
            setPositions[1] = true;
            m400.GetComponentInChildren<Renderer>().material.color = new Color(0, 128, 0);
            Debug.Log("Marker 400 calibrated successfully.");
        }
        if (isMarkerVisible(m600.transform.position)){
            positions[2] = m600.transform.position;
            positions[2].y += planeZOffset;
            setPositions[2] = true;
            m600.GetComponentInChildren<Renderer>().material.color = new Color(0, 128, 0);
            Debug.Log("Marker 600 calibrated successfully.");
        }
        if (isMarkerVisible(m800.transform.position)){
            positions[3] = m800.transform.position;
            positions[3].y += planeZOffset;
            setPositions[3] = true;
            m800.GetComponentInChildren<Renderer>().material.color = new Color(0, 128, 0);
            Debug.Log("Marker 800 calibrated successfully.");
        }
    }

    // Update is called once per frame
    void Update () {
        if (setPositions[0] && setPositions[1] && setPositions[2] && setPositions[3]){
            // Force the calibrated plane to be a rectangle
            Vector2 center = new Vector2(   positions[0].x * 0.25f +
                                            positions[1].x * 0.25f +
                                            positions[2].x * 0.25f +
                                            positions[3].x * 0.25f,
                                            positions[0].z * 0.25f +
                                            positions[1].z * 0.25f +
                                            positions[2].z * 0.25f +
                                            positions[3].z * 0.25f
                                         );
            if (positions[0].x - center.x <= positions[3].x - center.x)
                positions[3].x = positions[0].x;
            else
                positions[0].x = positions[3].x;

            if (positions[1].x - center.x <= positions[2].x - center.x)
                positions[2].x = positions[1].x;
            else
                positions[1].x = positions[2].x;

            if (positions[0].z - center.y <= positions[3].z - center.y)
                positions[3].z = positions[0].z;
            else
                positions[0].z = positions[3].z;
            if (positions[1].z - center.y <= positions[2].z - center.y)
                positions[2].z = positions[1].z;
            else
                positions[1].z = positions[2].z;
            float y = (positions[0].y + positions[1].y + positions[2].y + positions[3].y) / 4;
            center = new Vector3(center.x, y, center.y);

            Debug.Log("Calibration successful.");
            float width = positions[0].x - positions[1].x;
            float height = positions[3].z - positions[0].z;
            setupScene.calibrationDone(positions, center, width, height);
            m200.SetActive(false);
            m400.SetActive(false);
            m600.SetActive(false);
            m800.SetActive(false);
            this.enabled = false;
        }
	}
}
