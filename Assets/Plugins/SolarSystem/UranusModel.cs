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

	public override string GetTextureFilepath ()
	{
		return "";
	}

	public override double GetEclipticLongitude (double jd)
	{
		return AASUranus.EclipticLongitude(jd);
	}

	public override double GetEclipticLatitude (double jd)
	{
		return AASUranus.EclipticLatitude (jd);
	}

	public override double GetRadiusVector (double jd)
	{
		return AASUranus.RadiusVector (jd);
	}

	public override double GetSemidiameter ()
	{
		return AASDiameters.UranusSemidiameterB (vectorToEarthCorrected.Length());
	}

	protected override AASEllipticalObject GetEllipticalObject ()
	{
		return AASEllipticalObject.URANUS;
	}
}
