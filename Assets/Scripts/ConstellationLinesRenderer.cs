using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using MathUtils;

public class ConstellationLinesRenderer : MonoBehaviour {

	private Material lineMaterial;

	private Color lineColor = Color.red;

	private float lineWith = .2f;

	private SimController sim;

	private SkyModel skyModel;


	void Start () {
		sim = SimController.instance;
		skyModel = sim.skyModel;

		DrawConstellations ();
	}

	void DrawConstellations(){

		//CreateLineMaterial ();

		foreach (Constellation constellation in sim.skyModel.GetConstellations()) {
			GameObject goConst = new GameObject ();
			goConst.transform.parent = gameObject.transform;
			goConst.name = constellation.GetAbbr ();

			List<StarModel> stars = skyModel.GetStars ();
			int[] reverseMapping = skyModel.GetReverseMapping ();
			int i = 0;
			foreach(int[] line in constellation.GetLines()){

				GameObject go = new GameObject ();
				go.transform.parent = goConst.transform;
				LineRenderer renderer = go.AddComponent<LineRenderer> ();
				renderer.useWorldSpace = false; //essential to make 
				renderer.name = "line_"+i;
				renderer.SetVertexCount (2);
				//renderer.material = lineMaterial;
				renderer.SetColors (lineColor, lineColor);
				renderer.SetWidth (lineWith, lineWith);

				StarModel star1 = stars [reverseMapping [line [0]]];
				StarModel star2 = stars [reverseMapping [line [1]]];

				renderer.SetPosition(0, star1.GetEquatorialRectangularCoords () * (sim.radius + 1.0f));
				renderer.SetPosition(1, star2.GetEquatorialRectangularCoords () * (sim.radius + 1.0f));
				++i;
			}
		}

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


	void Update(){			

	}
		
}
