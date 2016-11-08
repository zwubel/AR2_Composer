//#define BYPASSNETWORK
using UnityEngine;
using System.Collections;

public class setupScene : MonoBehaviour {
    GameObject[] markerCubes;
    readInNetworkData networkData;
    Marker[] networkMarkers;
    Marker[] networkMarkersPrevFrame;
    bool markerArraySet = false;
    GameObject table;
    #if (BYPASSNETWORK)
    public bool bypassNetworkActivated = true;
#else
    public bool bypassNetworkActivated = false;
    #endif

    #if (BYPASSNETWORK)
    float frameIncrement = 0.0f;
    #endif

    [Header("Scene Settings")]    
    public int maxMarkers = 256;
    //public Vector3 planeScale = new Vector3(-0.14f, 0.782f, 0.08f);
    //public Vector3 planePosition = new Vector3(-0.146f, 0.782f, 0.084f);
    public float markerScale = 0.5f;
    [Header("Calibration Data")]
    public Vector3 calPos = new Vector3(0.0f, 0.0f, 0.0f);
    public Quaternion calRot = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
    public Vector3 calScale = new Vector3(1.0f, 1.0f, 1.0f);
    public Component OVR;

    public void calibrationDone(){
        calPos = OVR.transform.position;
        //calScale = OVR.transform.localScale;
        calRot = OVR.transform.rotation;
        //table.transform.localScale = calScale;
        table.transform.position = calPos;
        table.transform.localRotation = calRot;
    }

    // Use this for initialization
    void Start() {
        // Initialization
        markerCubes = new GameObject[maxMarkers];

        // Create parent object (plane and cubes are attached to this)
        GameObject parent = new GameObject();
        parent.transform.name = "Table Object";

        // Create plane
        table = GameObject.CreatePrimitive(PrimitiveType.Plane);
        table.transform.SetParent(parent.transform);
        table.transform.localScale = calScale;
        table.transform.position = calPos;
        table.transform.localRotation = calRot;

        // Create markers (cubes)
        for (int i = 0; i < maxMarkers; i++) {
            //markerCubes[i] = new GameObject();
            markerCubes[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            markerCubes[i].transform.SetParent(parent.transform);
            markerCubes[i].SetActive(false);
            markerCubes[i].transform.name = "Marker" + i;
            markerCubes[i].transform.localScale = new Vector3(markerScale, markerScale, markerScale);

            markerCubes[i].GetComponent<Renderer>().material.color = new Color(0, 255, 0);
        }
        networkData = gameObject.GetComponent<readInNetworkData>();
    }

    public void setMarkerArraySet(bool state) {
        markerArraySet = state;
    }

#if (BYPASSNETWORK)
    private void testMarkerMovement() {
        frameIncrement += 0.0001f;
        int numberOfMarkers = 5;
        networkMarkers = new Marker[numberOfMarkers];
        networkMarkers[0] = new Marker(1, -0.1f - frameIncrement, -0.1f - frameIncrement, 50.0f + frameIncrement * 10000);
        networkMarkers[1] = new Marker(2, -0.1f - frameIncrement, 0.1f + frameIncrement, 10.0f - frameIncrement * 10000);
        networkMarkers[2] = new Marker(3, 0.1f + frameIncrement, 0.1f + frameIncrement, 170.0f + frameIncrement * 10000);
        networkMarkers[3] = new Marker(4, 0.1f + frameIncrement, -0.1f - frameIncrement, 90.0f - frameIncrement * 10000);
        networkMarkers[4] = new Marker(-1, 0.0f, 0.0f, 0.0f);
        markerArraySet = true;
    }
#endif

    // Update is called once per frame
    void Update () {
#if (BYPASSNETWORK)
        testMarkerMovement();
#endif
        if (markerArraySet){
            networkMarkersPrevFrame = networkMarkers;
#if (!BYPASSNETWORK)
            networkMarkers = networkData.getMarkers();
#endif
            for (int i = 0; i < networkMarkers.Length; i++){
                Marker cur = networkMarkers[i];
                if (cur.getID() == -1){
                    break;
                }
                markerCubes[i].SetActive(true);
                markerCubes[i].transform.position = new Vector3(cur.getPosX(), 0.0f, cur.getPosY());
                markerCubes[i].transform.rotation = Quaternion.Euler(0.0f, cur.getAngle(), 0.0f);
            }
            for(int j = 0; j < networkMarkersPrevFrame.Length; j++){
                if(markerCubes[j] == null){
                    markerCubes[j].SetActive(false);
                }
            }
        }
    }

    IEnumerator wait(){
        yield return new WaitForSeconds(0.001f);
    }
}
