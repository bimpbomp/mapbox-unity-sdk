using System;
using System.Linq;

namespace Mapbox.Unity.Location
{
	/// <summary>
	///     <para>Smooths angles via a exponential moving average (EMA).</para>
	///     <para>https://en.wikipedia.org/wiki/Moving_average#Exponential_moving_average</para>
	///     <para>https://stackoverflow.com/questions/8450020/calculate-exponential-moving-average-on-a-queue-in-c-sharp</para>
	/// </summary>
	public class AngleSmoothingEMA : AngleSmoothingAbstractBase
	{
		private readonly double _alpha;


		public AngleSmoothingEMA()
		{
			_alpha = 2.0d / (_measurements + 1);
		}

		public override double Calculate()
		{
			// reverse order, _angles[0] is latest
			var angles = _angles.Reverse().ToArray();

			// since we cannot work directly on the angles (eg think about 355 and 5)
			// we convert to cartesian coordinates and apply filtering there
			// aproximation should be good enough for the use case of compass filtering
			// differences occur only at the 2nd or 3rd digit after the decimal point

			var sin = Math.Sin(angles[0] * DEG2RAD);
			var cos = Math.Cos(angles[0] * DEG2RAD);
			debugLogAngle(angles[0], Math.Atan2(sin, cos) * RAD2DEG);

			for (var i = 1; i < angles.Length; i++)
			{
				sin = (Math.Sin(angles[i] * DEG2RAD) - sin) * _alpha + sin;
				cos = (Math.Cos(angles[i] * DEG2RAD) - cos) * _alpha + cos;
				debugLogAngle(angles[i], Math.Atan2(sin, cos) * RAD2DEG);
			}

			// round, don't need crazy precision
			var finalAngle = Math.Round(Math.Atan2(sin, cos) * RAD2DEG, 2);
			debugLogAngle(finalAngle, finalAngle);
			// stay within [0..<360]
			finalAngle = finalAngle < 0 ? finalAngle + 360 : finalAngle >= 360 ? finalAngle - 360 : finalAngle;
			debugLogAngle(finalAngle, finalAngle);

			return finalAngle;
		}
	}
}
