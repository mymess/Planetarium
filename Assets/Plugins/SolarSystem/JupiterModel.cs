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

	public override string GetTextureFilepath ()
	{
		return "";
	}

	public override double GetEclipticLongitude (double jd)
	{
		return AASJupiter.EclipticLongitude (jd);
	}

	public override double GetEclipticLatitude (double jd)
	{
		return AASJupiter.EclipticLatitude (jd);
	}

	public override double GetRadiusVector (double jd)
	{
		return AASJupiter.RadiusVector (jd);
	}

	public override double GetSemidiameter ()
	{
		return AASDiameters.JupiterEquatorialSemidiameterB (vectorToEarthCorrected.Length ());
	}

	protected override AASEllipticalObject GetEllipticalObject ()
	{
		return AASEllipticalObject.JUPITER;
	}
}
