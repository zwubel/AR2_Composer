using UnityEngine;
using System.Collections;

public class setupScene : MonoBehaviour {
    GameObject[] markers;
    readInNetworkData networkData;
    Marker[] markerArray;
    
    [Header("Scene Settings")]
    public int maxMarkers = 16;
    public float planeScaleX = 1.0f;
    public float planeScaleZ = 0.5f;
    public float markerScale = 0.5f;

    // Use this for initialization
    void Start () {
        // Initialization
        Vector3 planeScale = new Vector3(planeScaleX, 1.0f, planeScaleZ);
        markers = new GameObject[maxMarkers];

        GameObject parent = new GameObject();
        parent.transform.position = new Vector3(0.88f, 0.61f, 2.63f);
        parent.transform.name = "Table Object";

        // Create plane
        GameObject table = GameObject.CreatePrimitive(PrimitiveType.Plane);
        table.transform.SetParent(parent.transform);       
        table.transform.localScale = planeScale;

        // Create markers (cubes)
        for (int i = 0; i < maxMarkers; i++) {
            GameObject cur = markers[i];
            cur = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cur.transform.SetParent(table.transform);
            cur.SetActive(false);
            cur.transform.name = "Marker" + i;
            cur.transform.localScale = new Vector3(markerScale, markerScale, markerScale);
            //cur.transform.Translate(0.0f, 1.0f, 0.0f);
            
            //GameObject text = new GameObject();
            //text.transform.SetParent(markers[i].transform);
            //text.AddComponent<TextMesh>().text = ""+i;
            //text.GetComponent<TextMesh>().GetComponent<Renderer>().material.color = new Color(0, 0, 0);
            //text.GetComponent<TextMesh>().name = "Label";
            //text.GetComponent<TextMesh>().transform.Rotate(90.0f, 0.0f, 0.0f);
            //text.GetComponent<TextMesh>().transform.position = new Vector3(-0.6f, 0.0f, 0.6f);
            cur.GetComponent<Renderer>().material.color = new Color(0, 255, 0);
        }
        networkData = gameObject.GetComponent<readInNetworkData>();
    }
	
	// Update is called once per frame
	void Update () {
        if (networkData.markersSet()){
            markerArray = networkData.getMarkers();
            for (int i = 0; i < markerArray.Length; i++){
                Marker cur = markerArray[i];
                if (cur.getID() == -1){
                    break;
                }
                markers[i].SetActive(true);
                markers[i].transform.position = new Vector3(cur.getPosX(), 0.0f, cur.getPosY());
                markers[i].transform.rotation = new Quaternion(0.0f, cur.getAngle(), 0.0f, 0.0f);
            }
        }
    }

    IEnumerator wait(){
        yield return new WaitForSeconds(0.001f);
    }
}
