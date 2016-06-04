using UnityEngine;
using System.Collections;

using AASharp;
using MathUtils;

public class UranusModel : PlanetModel {

	public UranusModel(double jd, LocationData location) : base(jd, location){
	}

	public override string GetName ()
	{
		return "Uranus";
	}

	public override double GetEclipticLongitude (double JD)
	{
		return AASUranus.EclipticLongitude(JD);
	}

	public override double GetEclipticLatitude (double JD)
	{
		return AASUranus.EclipticLatitude (JD);
	}

	protected override double GetRadiusVector (double JD)
	{
		return AASUranus.RadiusVector (JD);
	}

	public override double GetSemidiameter ()
	{
		return AASDiameters.UranusSemidiameterB (vectorToEarthCorrected.Length());
	}

	protected override AASEllipticalObject GetEllipticalObject ()
	{
		return AASEllipticalObject.URANUS;
	}

	public override double GetDistance ()
	{
		return AASUranus.RadiusVector (jdeCorrected);
	}
}
