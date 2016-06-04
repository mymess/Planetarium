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

	public override double GetEclipticLongitude (double JD)
	{
		return AASSaturn.EclipticLongitude (JD);
	}

	public override double GetEclipticLatitude (double JD)
	{
		return AASSaturn.EclipticLatitude (JD);
	}

	protected override double GetRadiusVector (double JD)
	{
		return AASSaturn.RadiusVector (JD);
	}

	public override double GetSemidiameter ()
	{
		return AASDiameters.SaturnPolarSemidiameterB (vectorToEarthCorrected.Length ());
	}

	protected override AASEllipticalObject GetEllipticalObject ()
	{
		return AASEllipticalObject.SATURN;
	}

	public override double GetDistance ()
	{
		return AASNeptune.RadiusVector (jdeCorrected);
	}

}
