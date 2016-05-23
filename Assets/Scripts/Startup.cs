using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;


using SimpleJSON;

//[InitializeOnLoad]
public class Startup {



	static Startup()
	{

		//StartCoroutine (LoadStarsAsync ());

		//LoadStars ();

			
	}


	static void LoadStars(){
		string filePath = "stars";

		TextAsset file = null;
		JSONNode testData = null;

		try{
			file = (TextAsset) Resources.Load<TextAsset>(filePath);

			if(file != null){
				testData = JSON.Parse(file.text); 

				Debug.Log("-->" + testData[0][0] );
				//Debug.Log( file.text );
			}else{

				Debug.Log("FILE NOT FOUND!!");
			}


		}catch(FileLoadException fex){
			Debug.Log("ERROR Opening file" + fex.ToString());
		}

	}

	static IEnumerator LoadStarsAsync(){

		JSONNode testData = null;


		ResourceRequest request = Resources.LoadAsync("stars");

		TextAsset file = (TextAsset)request.asset;


		if(file != null){
			testData = JSON.Parse(file.text); 

			Debug.Log("-->" + testData[0][0] );
			//Debug.Log( file.text );
		}else{

			Debug.Log("FILE NOT FOUND!!");
		}
			

		yield return request;


	}
}
