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

	public override string GetTextureFilepath ()
	{
		return "";
	}

	protected override AASEllipticalObject GetEllipticalObject ()
	{
		return AASEllipticalObject.VENUS;
	}
	public override double GetEclipticLongitude (double jd)
	{
		return AASVenus.EclipticLongitude (jd);
	}

	public override double GetEclipticLatitude (double jd)
	{
		return AASVenus.EclipticLatitude (jd);
	}

	public override double GetRadiusVector (double jd)
	{
		return AASVenus.RadiusVector (jd);
	}

	public override double GetSemidiameter ()
	{
		return AASDiameters.VenusSemidiameterA( vectorToEarthCorrected.Length() );
	}

}