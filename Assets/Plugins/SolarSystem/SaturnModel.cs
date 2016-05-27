using UnityEngine;
using System.Collections;

using AASharp;
using MathUtils;

public class SaturnModel : PlanetModel {

	public SaturnModel(double jd, LocationData location):base(jd, location){
	}

	public override string GetName ()
	{
		return "Saturn";
	}

	public override string GetTextureFilepath ()
	{
		return "";
	}

	public override double GetEclipticLongitude (double jd)
	{
		return AASSaturn.EclipticLongitude (jd);
	}

	public override double GetEclipticLatitude (double jd)
	{
		return AASSaturn.EclipticLatitude (jd);
	}

	public override double GetRadiusVector (double jd)
	{
		return AASSaturn.RadiusVector (jd);
	}

	public override double GetSemidiameter ()
	{
		return AASDiameters.SaturnPolarSemidiameterB (vectorToEarthCorrected.Length ());
	}

	protected override AASEllipticalObject GetEllipticalObject ()
	{
		return AASEllipticalObject.SATURN;
	}

}
