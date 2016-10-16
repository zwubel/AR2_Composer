using UnityEngine;
using System.Collections;

public class setupScene : MonoBehaviour {
    GameObject[] markers;
    int maxMarkers = 16;
    float xStretch = 10.0f;
    float zStretch = 5.0f;

    // Use this for initialization
    void Start () {
        Vector3 planeScale = new Vector3(xStretch, 1.0f, zStretch);
        markers = new GameObject[maxMarkers];
        GameObject table = GameObject.CreatePrimitive(PrimitiveType.Plane);
        table.transform.localScale = planeScale;
        for (int i = 0; i < maxMarkers; i++) {
            markers[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //markers[i].SetActive(false);
            markers[i].transform.name = "Marker" + i;
            markers[i].transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
            markers[i].transform.Translate(0.0f, 1.0f, 0.0f);

            //TextMesh textMesh = new TextMesh();
            GameObject text = new GameObject();
            text.transform.SetParent(markers[i].transform);
            text.AddComponent<TextMesh>().text = ""+i;
            text.GetComponent<TextMesh>().GetComponent<Renderer>().material.color = new Color(0, 0, 0);
            text.GetComponent<TextMesh>().name = "Label";
            text.GetComponent<TextMesh>().transform.Rotate(90.0f, 0.0f, 0.0f);
            text.GetComponent<TextMesh>().transform.position = new Vector3(-0.6f, 0.0f, 0.6f);
            markers[i].GetComponent<Renderer>().material.color = new Color(0, 255, 0);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
