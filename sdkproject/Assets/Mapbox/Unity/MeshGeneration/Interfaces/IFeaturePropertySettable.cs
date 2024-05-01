using System.Collections.Generic;

namespace Mapbox.Unity.MeshGeneration.Interfaces
{
	public interface IFeaturePropertySettable
	{
		void Set(Dictionary<string, object> props);
	}
}
