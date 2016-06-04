using UnityEngine;
using System.Collections;

using AASharp;
using MathUtils;

public class JupiterModel : PlanetModel {

	public JupiterModel(double jd, LocationData location):base(jd, location){
	}


	public override string GetName ()
	{
		return "Jupiter";
	}		

	public override double GetEclipticLongitude (double JD)
	{
		return AASJupiter.EclipticLongitude (JD);
	}

	public override double GetEclipticLatitude (double JD)
	{
		return AASJupiter.EclipticLatitude (JD);
	}

	protected override double GetRadiusVector (double JD)
	{
		return AASJupiter.RadiusVector (JD);
	}

	public override double GetSemidiameter ()
	{
		return AASDiameters.JupiterEquatorialSemidiameterB (vectorToEarthCorrected.Length ());
	}

	protected override AASEllipticalObject GetEllipticalObject ()
	{
		return AASEllipticalObject.JUPITER;
	}

	public override double GetDistance ()
	{
		return AASJupiter.RadiusVector (jdeCorrected);
	}
}
