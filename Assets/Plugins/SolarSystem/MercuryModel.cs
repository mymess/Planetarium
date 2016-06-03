using UnityEngine;
using System.Collections;
using AASharp;

using MathUtils;

public class MercuryModel : PlanetModel {


	public MercuryModel(double jd, LocationData location) : base (jd, location){
	}

	public override string GetName ()
	{
		return "Mercury";
	}		


	public override double GetEclipticLongitude (double jd)
	{
		return AASMercury.EclipticLongitude (jd);
	}

	public override double GetEclipticLatitude (double jd)
	{
		return AASMercury.EclipticLatitude (jd);
	}

	public override double GetRadiusVector (double jd)
	{
		return AASMercury.RadiusVector (jd);
	}

	public override double GetSemidiameter ()
	{
		return AASDiameters.MercurySemidiameterA (vectorToEarthCorrected.Length ());
	}

	protected override AASEllipticalObject GetEllipticalObject ()
	{
		return AASEllipticalObject.MERCURY;
	}

}
