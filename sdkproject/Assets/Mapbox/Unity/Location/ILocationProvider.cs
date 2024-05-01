using System;

namespace Mapbox.Unity.Location
{
	/// <summary>
	///     Implement ILocationProvider to send Heading and Location updates.
	/// </summary>
	public interface ILocationProvider
	{
		Location CurrentLocation { get; }
		event Action<Location> OnLocationUpdated;
	}
}
