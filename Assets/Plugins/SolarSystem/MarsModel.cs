using UnityEngine;
using System.Collections;
using AASharp;
using MathUtils;

public class MarsModel : PlanetModel {

	public MarsModel (double jd, LocationData location):base(jd, location){
	}

	public override string GetName ()
	{
		return "Mars";
	}

	public override double GetEclipticLongitude (double JD)
	{
		return AASMars.EclipticLongitude (JD);
	}

	public override double GetEclipticLatitude (double JD)
	{
		return AASMars.EclipticLatitude (JD);
	}

	protected override double GetRadiusVector (double JD)
	{
		return AASMars.RadiusVector (JD);
	}

	public override double GetSemidiameter ()
	{
		return AASDiameters.MarsSemidiameterB (vectorToEarthCorrected.Length());
	}

	protected override AASEllipticalObject GetEllipticalObject ()
	{
		return AASEllipticalObject.MARS;
	}

	public override double GetDistance ()
	{
		return AASMars.RadiusVector (jdeCorrected);
	}
}
