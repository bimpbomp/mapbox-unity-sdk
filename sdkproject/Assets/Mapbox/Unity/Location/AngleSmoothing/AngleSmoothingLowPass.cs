using System;
using System.Linq;
using UnityEngine;

namespace Mapbox.Unity.Location
{
	/// <summary>
	///     Smoothing via low pass filter
	/// </summary>
	public class AngleSmoothingLowPass : AngleSmoothingAbstractBase
	{
		[SerializeField]
		[Tooltip(
			"Factor to change smoothing. The lower the factor the slower the angle changes. '1' would be no smoothing")]
		[Range(0.01f, 0.9f)]
		private double _smoothingFactor = 0.5;


		public AngleSmoothingLowPass()
		{
		}


		public AngleSmoothingLowPass(double smoothingFactor)
		{
			_smoothingFactor = smoothingFactor;
		}


		public override double Calculate()
		{
			// reverse order, latest in _angles is at [0]
			var angles = _angles.Reverse().ToArray();

			// since we cannot work directly on the angles (eg think about 355 and 5)
			// we convert to cartesian coordinates and apply filtering there
			// aproximation should be good enough for the use case of compass filtering
			// differences occur only at the 2nd or 3rd digit after the decimal point

			var lastSin = Math.Sin(angles[0] * DEG2RAD);
			var lastCos = Math.Cos(angles[0] * DEG2RAD);

			debugLogAngle(angles[0], Math.Atan2(lastSin, lastCos) * RAD2DEG);

			for (var i = 1; i < angles.Length; i++)
			{
				var angle = angles[i];
				lastSin = _smoothingFactor * Math.Sin(angle * DEG2RAD) + (1 - _smoothingFactor) * lastSin;
				lastCos = _smoothingFactor * Math.Cos(angle * DEG2RAD) + (1 - _smoothingFactor) * lastCos;
				debugLogAngle(angles[i], Math.Atan2(lastSin, lastCos) * RAD2DEG);
			}

			// round, don't need crazy precision
			var finalAngle = Math.Round(Math.Atan2(lastSin, lastCos) * RAD2DEG, 2);
			debugLogAngle(finalAngle, finalAngle);
			// stay within [0..<360]
			finalAngle = finalAngle < 0 ? finalAngle + 360 : finalAngle >= 360 ? finalAngle - 360 : finalAngle;
			debugLogAngle(finalAngle, finalAngle);

			return finalAngle;
		}
	}
}
