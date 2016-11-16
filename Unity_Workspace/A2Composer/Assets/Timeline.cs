using UnityEngine;
using System.Collections;
using UnityEngine;
using System.IO;

public class Timeline : MonoBehaviour {
	public DirectoryInfo currentDirectory;
	public FileInformation[] files;
	public Rect r;
	public GameObject TimeLineScrollView;
	public GameObject prefabButton;
	// Use this for initialization

	void Start () {
		
		string dir = Application.dataPath + "/saves/";
		currentDirectory = new DirectoryInfo(dir);
		FileInfo[] fia = currentDirectory.GetFiles();
		//FileInfo[] fia = searchDirectory(di,searchPattern);
		files = new FileInformation[fia.Length];
		for(int f=0;f<fia.Length;f++){
				files[f] = new FileInformation(fia[f]);
				Debug.Log (files[f].fi.Name);
				GameObject button = (GameObject)Instantiate(prefabButton);
				button.transform.SetParent(TimeLineScrollView, false);
		}


		//Debug.Log (files.ToString);
		/*
		//FileInfo[] fia = searchDirectory(di,searchPattern);
		files = new FileInformation[fia.Length];
		for(int f=0;f<fia.Length;f++){
			if(fileTexture)
				files[f] = new FileInformation(fia[f],fileTexture);
			else
				files[f] = new FileInformation(fia[f]);
			*/


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
