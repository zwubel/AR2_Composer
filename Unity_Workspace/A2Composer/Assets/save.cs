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


public class save : MonoBehaviour {

	public XmlDocument doc;
	private String timeStamp; 
	string filepath;
	private readonly string projectPath = Application.dataPath;
	public XmlNode root;


	// Use this for initialization
	void Start () {
		
	}

	void saveScene	(){
		
		String timeStamp = System.DateTime.Now.ToString();

		timeStamp = timeStamp.Replace("/", "-");
		filepath  = Application.dataPath + "/saves/" + timeStamp +".xml"; 
		Debug.Log("TimeStamp: " + timeStamp);
		Debug.Log("Path: " + filepath);
		//Scene activeScene = EditorSceneManager.GetActiveScene();
		//SceneManager.UnloadScene (activeScene.buildIndex);

		//EditorSceneManager.SaveScene( activeScene );
		//EditorApplication.SaveAssets();

		UnityEngine.Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject)); //returns Object[]
		doc = new XmlDocument();
		doc.LoadXml("<AR2COMPOSER_SCENE>" +
					"<time>" +  
						timeStamp  +
					"</time>" +
					"</AR2COMPOSER_SCENE>"); 
		root = doc.DocumentElement;


		Console.WriteLine("Display the modified XML document...");
		Console.WriteLine(doc.OuterXml);
	

		traverseHirarchy(GameObject.Find("World"));
		Debug.Log ("Objects crawled!");
		Debug.Log ("Path: " + filepath);
		doc.Save (filepath);
	}


	void traverseHirarchy(GameObject obj){
		
		//if (obj.transform.childCount > 0) {
			Debug.Log (obj.name + " Mother of Childs");
			XmlNode newElem = doc.CreateNode("element", obj.name , "");  
			
			

			XmlNode newPos = doc.CreateNode("element", "Position" , "");  
			XmlNode newRotation = doc.CreateNode("element", "Rotation" , ""); 
			XmlNode newScale = doc.CreateNode("element", "Scale" , ""); 

			newPos.InnerText =obj.transform.localPosition.x +", "+  obj.transform.localPosition.y +", "+ obj.transform.localPosition.z ;
			newRotation.InnerText =obj.transform.localRotation.x +", "+  obj.transform.localRotation.y +", "+ obj.transform.localRotation.z ;
			newScale.InnerText =obj.transform.localScale.x +", "+  obj.transform.localScale.y +", "+ obj.transform.localScale.z ;

			newElem.AppendChild (newPos);
			newElem.AppendChild (newRotation);
			newElem.AppendChild (newScale);
			root.AppendChild(newElem);
		//}
		foreach (Transform child in obj.transform)
		{
			traverseHirarchy(child.gameObject);
		}


	}

	// Update is called once per fram
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Vector2 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Collider2D hitCollider = Physics2D.OverlapPoint (mousePosition);

			Debug.Log ("mouse pos " + mousePosition.x + " y " + mousePosition.y + " ");    
			saveScene ();
			/*
			if(hitCollider){
				selectorSprite.transform.position.x = hitCollider.transform.position.x;
				selectorSprite.transform.position.y = hitCollider.transform.position.y;    
				Debug.Log("Hit "+hitCollider.transform.name+" x"+hitCollider.transform.position.x+" y "+hitCollider.transform.position.y);    
			}
			*/
		}
	}
}