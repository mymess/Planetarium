using UnityEngine;
using System.Collections;

using AASharp;
using MathUtils;


public class NeptuneModel : PlanetModel {


	public NeptuneModel(double jd, LocationData location):base(jd, location){
		
	}

	public override string GetName ()
	{
		return "Neptune";
	}

	public override string GetTextureFilepath ()
	{
		return "";
	}

	public override double GetEclipticLongitude (double jd)
	{
		return AASNeptune.EclipticLongitude (jd);
	}

	public override double GetEclipticLatitude (double jd)
	{
		return AASNeptune.EclipticLatitude (jd);
	}

	public override double GetRadiusVector (double jd)
	{
		return AASNeptune.RadiusVector (jd);
	}

	public override double GetSemidiameter ()
	{
		return AASDiameters.NeptuneSemidiameterB ( vectorToEarthCorrected.Length ());
	}

	protected override AASEllipticalObject GetEllipticalObject ()
	{
		return AASEllipticalObject.NEPTUNE;
	}

}
