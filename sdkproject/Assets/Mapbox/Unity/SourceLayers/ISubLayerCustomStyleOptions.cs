using UnityEngine;

namespace Mapbox.Unity.Map
{
	public interface ISubLayerCustomStyleOptions
	{
		/// <summary>
		///     Gets or sets the top material.
		/// </summary>
		/// <value>The top material.</value>
		Material TopMaterial { get; set; }

		/// <summary>
		///     Gets or sets the side material.
		/// </summary>
		/// <value>The side material.</value>
		Material SideMaterial { get; set; }
	}
}
