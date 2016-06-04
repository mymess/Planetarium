using UnityEngine;
using System.Collections;
using AASharp;

using MathUtils;

public class MoonModel : SolarSystemBody {


	public MoonModel(double jd, LocationData location): base (jd, location) {		
		Update ();
	}



	protected override void Update ()
	{
		CalculateEquatorialPosition ();

		CalculateTopocentricPosition ();
	}


	private void CalculateEquatorialPosition(){
		DegreesAngle ecLon = new DegreesAngle(AASMoon.EclipticLongitude (jd));
		DegreesAngle ecLat = new DegreesAngle(AASMoon.EclipticLatitude (jd));

		double epsilon = AASNutation.TrueObliquityOfEcliptic (jd);

		AAS2DCoordinate eq = AASCoordinateTransformation.Ecliptic2Equatorial (ecLon.Get(), ecLat.Get(), epsilon);

		equatorialCoords.RA = new HourAngle (eq.X);
		equatorialCoords.Declination = new DegreesAngle (eq.Y);
	}

	public override string GetName ()
	{
		return "Moon";
	}




	protected override double GetRadiusVector (double JD)
	{
		return AASMoon.RadiusVector (JD);
	}

	public override double GetDistance(){				
		return AASMoon.RadiusVector (jd);
	}
	//moon semidiameter in arc seconds
	public override double GetSemidiameter(){
		return AASDiameters.TopocentricMoonSemidiameter (
			GetDistance (),
			equatorialCoords.Declination.Get (),
			localHourAngle,
			location.latitude,
			location.altitude);
	}


}
