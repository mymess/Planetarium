using UnityEngine;
using System.Collections;
using AASharp;

using MathUtils;

public class VenusModel : PlanetModel{


	public VenusModel(double jd, LocationData location) : base (jd, location){
		
	}

	public override string GetName ()
	{
		return "Venus";
	}
		
	protected override AASEllipticalObject GetEllipticalObject ()
	{
		return AASEllipticalObject.VENUS;
	}
	public override double GetEclipticLongitude (double JD)
	{
		return AASVenus.EclipticLongitude (JD);
	}

	public override double GetEclipticLatitude (double JD)
	{
		return AASVenus.EclipticLatitude (JD);
	}

	protected override double GetRadiusVector (double JD)
	{
		return AASVenus.RadiusVector (JD);
	}

	public override double GetSemidiameter ()
	{
		return AASDiameters.VenusSemidiameterA( vectorToEarthCorrected.Length() );
	}

	public override double GetDistance ()
	{
		return AASVenus.RadiusVector (jdeCorrected);
	}
}