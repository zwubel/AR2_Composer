#define DEBUGGING
using UnityEngine;
using System.Collections;

public class setupScene : MonoBehaviour {
    GameObject[] markerCubes;
    readInNetworkData networkData;
    Marker[] networkMarkers;
    Marker[] networkMarkersPrevFrame;
    bool markerArraySet = false;
    
    #if(DEBUGGING)
    float frameIncrement = 0.0f;
    #endif

    [Header("Scene Settings")]
    public int maxMarkers = 256;
    public float planeScaleX = 1.0f;
    public float planeScaleZ = 0.5f;
    public float markerScale = 0.5f;

    // Use this for initialization
    void Start() {
        // Initialization
        Vector3 planeScale = new Vector3(planeScaleX, 1.0f, planeScaleZ);
        markerCubes = new GameObject[maxMarkers];

        // Create parent object (plane and cubes are attached to this)
        GameObject parent = new GameObject();
        parent.transform.position = new Vector3(0.88f, 0.61f, 2.63f);
        parent.transform.name = "Table Object";

        // Create plane
        GameObject table = GameObject.CreatePrimitive(PrimitiveType.Plane);
        table.transform.SetParent(parent.transform);
        table.transform.localScale = planeScale;
        table.transform.position = new Vector3(0.0f, -1.0f * markerScale / 2, 0.0f);

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

    #if(DEBUGGING)
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
        #if (DEBUGGING)
        testMarkerMovement();
        #endif
        if (markerArraySet){
            networkMarkersPrevFrame = networkMarkers;
            #if (!DEBUGGING)
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
