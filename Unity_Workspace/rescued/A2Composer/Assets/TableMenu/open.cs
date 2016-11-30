﻿using UnityEngine;
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
	public GameObject initCube;
	//public bool visible;
	// Use this for initialization
	void Start () {
		//openDialog.SetActive(false);
	


		String filePath = "/Users/PaulBerning/11-24-2016 10-42-44.xml";

		string xmlString = System.IO.File.ReadAllText(filePath);

		XmlDocument xml = new XmlDocument();
		xml.LoadXml(xmlString);
		XmlNode root = xml.FirstChild;
	
		XmlNodeList children = root.ChildNodes;
		
		crawlXML (children);



	}

	void crawlXML( XmlNodeList nodes ){
		for(int i=0; i< nodes.Count; i++){
			//Debug.Log(nodes[i].Name);
			if (nodes [i].Name.Contains ("Cube")) {
				Debug.Log ("CUBE FOUND BITCH");
				GameObject tmp = Instantiate(initCube);
				String tmpName = "Cube" + i;
				tmp.name = tmpName;
				tmp.SetActive (true);
				float PosX=0.0f;
				float PosY=0.0f; 
				float PosZ=0.0f;

				float RotX = 0.0f;
				float RotY = 0.0f;
				float RotZ = 0.0f;

				float ScaleX = 0.0f;
				float ScaleY = 0.0f;
				float ScaleZ = 0.0f;

				foreach (XmlNode node in nodes[i]) {
					if (node.Name == "PositionX") {
						PosX = float.Parse (node.InnerText,System.Globalization.CultureInfo.CurrentCulture); 
						//Debug.Log(node.InnerText + ", " + PosX);
					}
					if (node.Name == "PositionY") {
						PosY= float.Parse (node.InnerText,System.Globalization.CultureInfo.InvariantCulture); 
						//Debug.Log(node.InnerText + ", " + PosY);
					}
					if (node.Name == "PositionZ") {
						PosZ = float.Parse (node.InnerText,System.Globalization.CultureInfo.InvariantCulture);
						//Debug.Log(node.InnerText + ", " + PosZ);
					}

					if (node.Name == "RotationX") {
						RotX = float.Parse (node.InnerText,System.Globalization.CultureInfo.InvariantCulture); 
					}
					if (node.Name == "RotationY") {
						RotY= float.Parse (node.InnerText,System.Globalization.CultureInfo.InvariantCulture); 
					}
					if (node.Name == "RotationZ") {
						RotZ = float.Parse (node.InnerText,System.Globalization.CultureInfo.InvariantCulture);
					}

					if (node.Name == "ScaleX") {
						ScaleX = float.Parse (node.InnerText,System.Globalization.CultureInfo.InvariantCulture); 
						if (ScaleX == 0) ScaleX = 1;
					}
					if (node.Name == "ScaleY") {
						ScaleY = float.Parse (node.InnerText,System.Globalization.CultureInfo.InvariantCulture); 
						if (ScaleY == 0) ScaleX = 1;
					}
					if (node.Name == "ScaleZ") {
						ScaleZ = float.Parse (node.InnerText,System.Globalization.CultureInfo.InvariantCulture);
						if (ScaleZ == 0) ScaleX = 1;
					}

					tmp.transform.localPosition = (new Vector3(PosX,PosY,PosZ));

					tmp.transform.localRotation = Quaternion.Euler (RotX, RotY, RotZ); // need to be bugfixed 
					tmp.transform.localScale = (new Vector3 (ScaleX, ScaleY, ScaleZ));
				}
			}
			crawlXML(nodes [i].ChildNodes);
		}

	}
	public void setPath(){

			Debug.Log (this.gameObject.name);
	}
	
	// Update is called once per frame
	void Update () {
		/*if (Input.GetMouseButtonDown (0)) {
			Vector2 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Collider2D hitCollider = Physics2D.OverlapPoint (mousePosition);
//			if (openDialog.active == true) {
//				openDialog.SetActive (false);
//			} else
//				openDialog.SetActive (true);
			Debug.Log ("Opening at pos " + mousePosition.x + " y " + mousePosition.y + " ");  
		}*/
	}
}
