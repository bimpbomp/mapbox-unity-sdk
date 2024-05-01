using Mapbox.Unity.MeshGeneration.Data;

namespace Mapbox.Unity.MeshGeneration.Filters
{
	public interface ILayerFeatureFilterComparer
	{
		bool Try(VectorFeatureUnity feature);
	}

	public class FilterBase : ILayerFeatureFilterComparer
	{
		public virtual string Key => "";

		public virtual bool Try(VectorFeatureUnity feature)
		{
			return true;
		}

		public virtual void Initialize()
		{
		}
	}
}
