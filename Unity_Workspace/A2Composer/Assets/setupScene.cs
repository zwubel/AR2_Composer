using UnityEngine;
using System.Collections;

public class setupScene : MonoBehaviour {
    GameObject[] markerCubes;
    readInNetworkData networkData;
    Marker[] networkMarkers;
    Marker[] networkMarkersPrevFrame;
    bool markerArraySet = false;
    GameObject table;
    public bool bypassNetwork = true;
    float frameIncrement = 0.0f;
    GameObject parent;
    private Vector3[] tablePositions;

    [Header("Scene Settings")]
    public int maxMarkers = 100;
    //public Vector3 planeScale = new Vector3(-0.14f, 0.782f, 0.08f);
    //public Vector3 planePosition = new Vector3(-0.146f, 0.782f, 0.084f);
    public float markerScale = 0.5f;
    [Header("Calibration")]
    public TableCalibration tableCalib;

    Mesh createPlane(Vector3[] positions){
        Mesh m = new Mesh();
        m.name = "ScriptedMesh";
        m.vertices = new Vector3[] {positions[1], positions[0], positions[3], positions[2]};
        m.uv = new Vector2[] {
         new Vector2 (0, 0),
         new Vector2 (0, 1),
         new Vector2(1, 1),
         new Vector2 (1, 0)
     };
        m.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        m.RecalculateNormals();
        return m;
    }

    public void calibrationDone(Vector3[] markerPositions){
        tablePositions = markerPositions;
        table = GameObject.CreatePrimitive(PrimitiveType.Plane);
        table.GetComponent<MeshFilter>().mesh = createPlane(tablePositions);
        table.transform.SetParent(parent.transform);        

        //GameObject plane = new GameObject("Plane");
        //MeshFilter meshFilter = (MeshFilter)plane.AddComponent(typeof(MeshFilter));
        //meshFilter.mesh = CreateMesh(1, 0.2f);
        //MeshRenderer renderer = plane.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        //renderer.material.shader = Shader.Find("Particles/Additive");
        //Texture2D tex = new Texture2D(1, 1);
        //tex.SetPixel(0, 0, Color.green);
        //tex.Apply();
        //renderer.material.mainTexture = tex;
        //renderer.material.color = Color.green;
    }

    // Use this for initialization
    void Start() {
        tablePositions = new Vector3[4];
        tableCalib.enabled = false;
        // Initialization
        markerCubes = new GameObject[maxMarkers];

        // Create parent object (plane and cubes are attached to this)
        parent = new GameObject();
        parent.transform.name = "Table Object";

        // Create markers (cubes)
        for (int i = 0; i < maxMarkers; i++) {
            markerCubes[i] = initializeMarker(i);
            markerCubes[i].GetComponent<Renderer>().material.color = new Color(0, 255, 0);
        }
        networkData = gameObject.GetComponent<readInNetworkData>();
        tableCalib.enabled = true;
    }

    public void setMarkerArraySet(bool state) {
        markerArraySet = state;
    }

    private GameObject initializeMarker(int index){
        GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
        marker.transform.SetParent(parent.transform);
        marker.SetActive(false);
        marker.transform.name = "Marker" + index;
        marker.transform.localScale = new Vector3(markerScale, markerScale, markerScale);
        return marker;
    }

    private void simulateMarkerMovement() {
        frameIncrement += 0.0001f;
        networkMarkers = new Marker[5];
        networkMarkers[0] = new Marker(1, -0.1f - frameIncrement,  -0.1f - frameIncrement,   50.0f + frameIncrement * 10000,    1);
        networkMarkers[1] = new Marker(2, -0.1f - frameIncrement,   0.1f + frameIncrement,   10.0f - frameIncrement * 10000,    1);
        networkMarkers[2] = new Marker(3,  0.1f + frameIncrement,   0.1f + frameIncrement,  170.0f + frameIncrement * 10000,    1);
        networkMarkers[3] = new Marker(4,  0.1f + frameIncrement,  -0.1f - frameIncrement,   90.0f - frameIncrement * 10000,    1);
        networkMarkers[4] = new Marker(-1, 0.0f,                    0.0f,                     0.0f,                             0);
        markerArraySet = true;
    }

    // Returns the position on the plane for the tracked (normalized) marker position
    private Vector3 getCalibratedMarkerPos(Vector3 position){
        // Linear interpolation of X
        float xMin = tablePositions[1].x;
        float xMax = tablePositions[0].x;
        float newX = xMin + position.x * (xMax - xMin);

        // Linear interpolation of Y (Z in unity)
        float yMin = tablePositions[1].z;
        float yMax = tablePositions[2].z;
        float newY = yMin + position.z * (yMax - yMin);

        // Linear interpolation of Z (Y in unity)
        float z1 = tablePositions[0].y * ((xMax - xMin) / (xMax - newX)) + tablePositions[1].y * ((xMax - xMin) / (newX - xMin));
        float z2 = tablePositions[3].y * ((xMax - xMin) / (xMax - newX)) + tablePositions[2].y * ((xMax - xMin) / (newX - xMin));
        float newZ = z2 * ((yMax - yMin) / (yMax - newY)) + z1 * ((yMax - yMin) / (newY - yMin));
        return new Vector3(newX, newZ, newY);
    }

    // Update is called once per frame
    void Update () {
        if (bypassNetwork) {
            simulateMarkerMovement();
            networkMarkers = networkData.getMarkers();
        }else if (markerArraySet){
            networkMarkers = networkData.getMarkers();
            networkMarkersPrevFrame = networkMarkers;
            for (int i = 0; i < networkMarkers.Length; i++){
                Marker cur = networkMarkers[i];
                if(cur != null) { 
                    if (cur.getID() == -1){
                        break;
                    }
                    int status = networkMarkers[i].getStatus();
                    if (status == 1)
                        markerCubes[i].SetActive(true);
                    else if(status == 0)
                        markerCubes[i].SetActive(false);
                    markerCubes[i].transform.position = getCalibratedMarkerPos(new Vector3(cur.getPosX(), 0.0f, cur.getPosY()));
                    markerCubes[i].transform.rotation = Quaternion.Euler(0.0f, cur.getAngle(), 0.0f);
                }
            }
            // Check if any markers have been deleted
            for(int j = 0; j < networkMarkersPrevFrame.Length - 1; j++){
                if (networkMarkersPrevFrame[j] == null && networkMarkers[j] != null)
                    markerCubes[j] = initializeMarker(j); //Marker has been deleted, reinitialize GameObject
            }
        }
    }
}