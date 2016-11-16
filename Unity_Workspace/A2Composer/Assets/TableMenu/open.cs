using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class open : MonoBehaviour {
	public GameObject openDialog;
	//public bool visible;
	// Use this for initialization
	void Start () {
		openDialog.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Vector2 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Collider2D hitCollider = Physics2D.OverlapPoint (mousePosition);
			if (openDialog.active == true) {
				openDialog.SetActive (false);
			} else
				openDialog.SetActive (true);
			Debug.Log ("Opening at pos " + mousePosition.x + " y " + mousePosition.y + " ");  
		}
	}
}
