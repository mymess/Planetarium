using UnityEngine;

using System.Collections;



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


		sim = SimController.simController;

		skyModel = sim.skyModel;


		CreatePoints ();

	}


	void CreatePoints(){
		
		points = new ParticleSystem.Particle[ skyModel.getStars().Count ];


		currentRadius = sim.radius;
		currentStarSize = maxStarSize;

		for(int i = 0; i < skyModel.getStars().Count; i++){
			StarModel star = skyModel.getStars () [i];
		
			float x = Mathf.Cos(star.dec *Mathf.Deg2Rad) * Mathf.Sin(star.ra*15.0f *Mathf.Deg2Rad);
			float y = Mathf.Sin(star.dec *Mathf.Deg2Rad);
			float z = -Mathf.Cos(star.dec *Mathf.Deg2Rad) * Mathf.Cos(star.ra*15.0f *Mathf.Deg2Rad);
					
			points [i].position = star.GetNormalizedPosition() * sim.radius;



			points[i].startColor = new Color(Random.Range (0.0f,1.0f), Random.Range (0.0f,1.0f), Random.Range (0.0f,1.0f), 1.0f);


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

		if (currentStarSize != maxStarSize) {
			CreatePoints ();
		}

		ps.SetParticles(points, points.Length);
	}
}