using System;

namespace Mapbox.Editor
{
	[Serializable]
	public class FeatureTreeElement : TreeElement
	{
		public string Name;
		public string Type;
		public bool isActive;

		public FeatureTreeElement(string name, int depth, int id) : base(name, depth, id)
		{
			isActive = true;
		}
	}
}
