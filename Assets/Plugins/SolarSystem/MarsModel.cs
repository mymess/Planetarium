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

	public override double GetEclipticLongitude (double jd)
	{
		return AASMars.EclipticLongitude (jd);
	}

	public override double GetEclipticLatitude (double jd)
	{
		return AASMars.EclipticLatitude (jd);
	}

	public override double GetRadiusVector (double jd)
	{
		return AASMars.RadiusVector (jd);
	}

	public override double GetSemidiameter ()
	{
		return AASDiameters.MarsSemidiameterB (vectorToEarthCorrected.Length());
	}

	protected override AASEllipticalObject GetEllipticalObject ()
	{
		return AASEllipticalObject.MARS;
	}

}
