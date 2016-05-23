using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineRendererTest : MonoBehaviour {

	private Material lineMaterial;

	public GameObject lineDrawPrefabs; // this is where we put the prefabs object

	private bool isMousePressed;
	private GameObject lineDrawPrefab;
	private LineRenderer lr;
	private LineRenderer[] lrs;
	private Vector3[] drawPoints = new Vector3[100];

	// Use this for initialization
	void Start () {
		isMousePressed = false;

		//Test1 ();
		Test4();
		//Test2();

		//Test3 ();
	}

	void Test3(){
		GameObject go = new GameObject ();

		for (int i = 0; i < 10; ++i) {
			
			LineRenderer renderer = UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(gameObject, "Assets/Scripts/LineRendererTest.cs (32,28)", "const_" + i) as LineRenderer;
			renderer.material = new Material (Shader.Find ("Particles/Additive"));
			renderer.SetVertexCount (2);

			float x = Random.Range(-100.0f, 100.0f);
			float y =Random.Range(-100.0f, 100.0f);;
			float z =Random.Range(-100.0f, 100.0f);;

			renderer.SetPosition (0, Vector3.zero);
			renderer.SetPosition (1, new Vector3(x, y ,z ));


		}

	}


	void Test2(){
		GameObject go = new GameObject ();
		for (int i = 0; i < 10; ++i) {
			LineRenderer renderer = go.AddComponent<LineRenderer>();
			renderer.material = new Material (Shader.Find ("Particles/Additive"));
			renderer.SetVertexCount (100);
			for(int j=0; j<100; ++j){
				float x = Random.Range(-100.0f, 100.0f);
				float y =Random.Range(-100.0f, 100.0f);;
				float z =Random.Range(-100.0f, 100.0f);;

				drawPoints[j] = new Vector3(x, y, z);

			}
			float c1 = Random.Range(0.0f, 1.0f);
			float c2 = Random.Range(0.0f, 1.0f);
			float c3 = Random.Range(0.0f, 1.0f);

			renderer.SetColors (new Color(c1, c2, c3), new Color(c2, c1, c3) );
			renderer.SetPositions (drawPoints);

		}
	}

	void Test1(){
		lr = gameObject.AddComponent<LineRenderer>();

		lr.material = new Material (Shader.Find ("Particles/Additive"));

		lr.SetVertexCount (100);

		for(int i=0; i<100; ++i){
			float x = Random.Range(-100.0f, 100.0f);
			float y =Random.Range(-100.0f, 100.0f);;
			float z =Random.Range(-100.0f, 100.0f);;

			drawPoints[i] = new Vector3(x, y, z);

		}
		lr.SetColors (Color.blue, Color.red);
		lr.SetPositions (drawPoints);

	}

	void Test4(){
		GameObject go = new GameObject ();

		go.transform.parent = gameObject.transform;

		lr = go.AddComponent<LineRenderer>();

		lr.material = new Material (Shader.Find ("Particles/Additive"));

		lr.SetVertexCount (100);

		for(int i=0; i<100; ++i){
			float x = Random.Range(-100.0f, 100.0f);
			float y =Random.Range(-100.0f, 100.0f);;
			float z =Random.Range(-100.0f, 100.0f);;

			drawPoints[i] = new Vector3(x, y, z);

		}
		lr.SetColors (Color.blue, Color.red);
		lr.SetPositions (drawPoints);

	}





	// Update is called once per frame
	void Update () {
		
	}
		


	void DrawLine(Vector3 start , Vector3 end, Color color){
		// Will be called after all regular rendering is done

		CreateLineMaterial ();
		// Apply the line material
		lineMaterial.SetPass (0);

		GL.PushMatrix ();
		GL.LoadIdentity ();
		// Set transformation matrix for drawing to
		// match our transform
		GL.MultMatrix (transform.localToWorldMatrix);

		// Draw lines
		GL.Begin (GL.LINES);
		// Vertex colors change from red to green
		GL.Color (color);
		// One vertex at transform position
		GL.Vertex3 (start.x, start.y, start.z);			
		GL.Vertex3 (end.x, end.y, end.z);

		GL.End ();
		GL.PopMatrix ();

	}

	void CreateLineMaterial ()
	{
		if (!lineMaterial)
		{
			// Unity has a built-in shader that is useful for drawing
			// simple colored things.
			var shader = Shader.Find ("Hidden/Internal-Colored");
			lineMaterial = new Material (shader);
			lineMaterial.hideFlags = HideFlags.HideAndDontSave;
			// Turn on alpha blending
			lineMaterial.SetInt ("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
			lineMaterial.SetInt ("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
			// Turn backface culling off
			lineMaterial.SetInt ("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
			// Turn off depth writes
			lineMaterial.SetInt ("_ZWrite", 0);
		}
	}

}
