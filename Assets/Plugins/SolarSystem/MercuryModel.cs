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


	public override double GetEclipticLongitude (double JD)
	{
		return AASMercury.EclipticLongitude (JD);
	}

	public override double GetEclipticLatitude (double JD)
	{
		return AASMercury.EclipticLatitude (JD);
	}

	protected override double GetRadiusVector (double JD)
	{
		return AASMercury.RadiusVector (JD);
	}

	public override double GetSemidiameter ()
	{
		return AASDiameters.MercurySemidiameterA (vectorToEarthCorrected.Length ());
	}

	protected override AASEllipticalObject GetEllipticalObject ()
	{
		return AASEllipticalObject.MERCURY;
	}

	public override double GetDistance ()
	{
		return AASMercury.RadiusVector (jdeCorrected);
	}
}
