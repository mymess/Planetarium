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

	public override double GetEclipticLongitude (double JD)
	{
		return AASNeptune.EclipticLongitude (JD);
	}

	public override double GetEclipticLatitude (double JD)
	{
		return AASNeptune.EclipticLatitude (JD);
	}

	protected override double GetRadiusVector (double JD)
	{
		return AASNeptune.RadiusVector (JD);
	}

	public override double GetSemidiameter ()
	{
		return AASDiameters.NeptuneSemidiameterB ( vectorToEarthCorrected.Length ());
	}

	protected override AASEllipticalObject GetEllipticalObject ()
	{
		return AASEllipticalObject.NEPTUNE;
	}

	public override double GetDistance ()
	{
		return AASNeptune.RadiusVector (jdeCorrected);
	}
}
