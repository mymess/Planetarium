using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using MathUtils;

public class ConstellationsRenderer : MonoBehaviour {

	private SimController sim ;

	private SkyModel skyModel;

	private static Material lineMaterial;

	public Color constellationColor = Color.green;


	private GameObject go;

	// Use this for initialization
	void Start () {
		sim = SimController.instance;
		skyModel = sim.skyModel;
		go = new GameObject ();
	}


	void OnPostRender(){
		DrawConstellations ();
	}


	void DrawConstellations(){
		CreateLineMaterial ();
		lineMaterial.SetPass( 0 );


		double jd = SimController.instance.GetJD ();
		LocationData location = SimController.instance.location;


		Transform zero = go.transform;
		zero.position = Vector3.zero;
		zero.rotation = Quaternion.identity;
		zero.localScale = new Vector3 (1.0f, 1.0f, 1.0f); 

		GL.MultMatrix (zero.worldToLocalMatrix);


		List<StarModel> stars = skyModel.GetStars ();
		int[] reverseMapping = skyModel.GetReverseMapping ();

		GL.PushMatrix ();
		GL.Begin( GL.LINES );

		GL.Color( constellationColor );
		foreach (Constellation constellation in skyModel.GetConstellations()) {
			foreach (int[] line in constellation.GetLines()) {
				StarModel star1 = stars [reverseMapping [line [0]]];
				StarModel star2 = stars [reverseMapping [line [1]]];
				GL.Vertex (star1.GetEquatorialRectangularCoords()  * sim.radius);
				GL.Vertex (star2.GetEquatorialRectangularCoords() * sim.radius);
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



}
