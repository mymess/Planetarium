using UnityEngine;

using System.Collections;

using MathUtils;

public class StarRenderer : MonoBehaviour {

	private ParticleSystem ps;

	public float maxStarSize = 50.0f;

	private float minStarSize = 0.005f;

	private float exponent = Mathf.Pow(100.0f, .2f);

	private float currentStarSize;

	private ParticleSystem.Particle[] points;

	private SkyModel skyModel;

	private float currentRadius;

	private SimController sim;

	private ParticleEmitter emitter;


	// Use this for initialization
	void Start () {
		ps = gameObject.GetComponent<ParticleSystem> ();

		ParticleEmitter emitter = ps.GetComponent<ParticleEmitter> ();


		sim = SimController.instance;

		skyModel = sim.skyModel;


		CreatePoints ();

	}


	void CreatePoints(){
		
		points = new ParticleSystem.Particle[ skyModel.GetStars().Count ];


		currentRadius = sim.radius;
		currentStarSize = maxStarSize;

		for(int i = 0; i < skyModel.GetStars().Count; i++){
			StarModel star = skyModel.GetStars () [i];
		

			points[i].position = star.GetEquatorialRectangularCoords() * sim.radius;

			points[i].startColor = star.GetStarRGB ();

			points [i].startSize = maxStarSize * Mathf.Pow(star.getClampedMagnitude (), exponent) + minStarSize;

			points[i].velocity = Vector3.zero;
			points[i].angularVelocity = 0.0f;
			points[i].rotation = 0.0f;


		}

		ps.maxParticles = Mathf.RoundToInt(points.Length);
		ps.SetParticles (points, points.Length);


	}
	
	// Update is called once per frame
	void Update () {
		if (ps == null || points == null) {
			return;
		}

		ps.SetParticles(points, points.Length);
	}

	void UpdateStars(){
		double jd = SimController.instance.GetJD ();
		LocationData location = SimController.instance.GetLocation ();
	}

}