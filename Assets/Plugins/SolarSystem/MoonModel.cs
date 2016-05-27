using UnityEngine;
using System.Collections;
using AASharp;

using MathUtils;

public class MoonModel : SolarSystemBody {


	public MoonModel(double jd, LocationData location) {
		this.JD = jd;
		this.location = location;
		Update ();
	}



	protected override void Update ()
	{
		CalculateEquatorialPosition ();

		CalculateTopocentricPosition ();
	}


	private void CalculateEquatorialPosition(){
		DegreesAngle ecLon = new DegreesAngle(AASMoon.EclipticLongitude (JD));
		DegreesAngle ecLat = new DegreesAngle(AASMoon.EclipticLatitude (JD));

		double epsilon = AASNutation.TrueObliquityOfEcliptic (JD);

		AAS2DCoordinate eq = AASCoordinateTransformation.Ecliptic2Equatorial (ecLon.Get(), ecLat.Get(), epsilon);

		equatorialCoords.RA = new HourAngle (eq.X);
		equatorialCoords.Declination = new DegreesAngle (eq.Y);
	}

	public override string GetName ()
	{
		return "Moon";
	}

	public override string GetTextureFilepath ()
	{
		return "";
	}


	public double GetDiameter(){
		//TODO: cambiar por el Topocentric
		return 2 * AASDiameters.GeocentricMoonSemidiameter (AASMoon.RadiusVector(JD));
	}
}
