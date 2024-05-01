using Mapbox.Unity.MeshGeneration.Data;

namespace Mapbox.Unity.MeshGeneration.Modifiers
{
	public interface IReplacementCriteria
	{
		bool ShouldReplaceFeature(VectorFeatureUnity feature);
	}
}
