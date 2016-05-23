using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConstellationsRenderer : MonoBehaviour {

	private SimController sim ;

	private SkyModel skyModel;

	private static Material lineMaterial;

	public Color constellationColor = Color.red;


	private GameObject go;

	// Use this for initialization
	void Start () {
		sim = SimController.simController;
		skyModel = sim.skyModel;
		go = new GameObject ();

	}


	void OnPostRender(){
		DrawConstellations ();
	}


	void DrawConstellations(){
		CreateLineMaterial ();
		lineMaterial.SetPass( 0 );

		Transform zero = go.transform;
		zero.position = Vector3.zero;
		zero.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
		zero.localScale = new Vector3 (1.0f, 1.0f, 1.0f); 

		GL.MultMatrix (zero.worldToLocalMatrix);


		List<StarModel> stars = skyModel.getStars ();
		int[] reverseMapping = skyModel.getReverseMapping ();

		GL.PushMatrix ();
		GL.Begin( GL.LINES );

		GL.Color( constellationColor );
		foreach (Constellation constellation in skyModel.getConstellations()) {
			foreach (int[] line in constellation.GetLines()) {
				StarModel star1 = stars [reverseMapping [line [0]]];
				StarModel star2 = stars [reverseMapping [line [1]]];
				GL.Vertex (GetPosition (star1));
				GL.Vertex (GetPosition (star2));
			}

		}

		GL.End();
		GL.PopMatrix();


	}



	static void CreateLineMaterial ()
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

	Vector3 GetPosition(StarModel star){
		float x = Mathf.Cos(star.dec *Mathf.Deg2Rad) * Mathf.Sin(star.ra*15.0f *Mathf.Deg2Rad);
		float y = Mathf.Sin(star.dec *Mathf.Deg2Rad);
		float z = Mathf.Cos(star.dec *Mathf.Deg2Rad) * Mathf.Cos(star.ra*15.0f *Mathf.Deg2Rad);

		return new Vector3 (x, y, z) * sim.radius;
	}



}
