using System;

namespace Mapbox.Unity.Utilities
{
	public class NodeEditorElementAttribute : Attribute
	{
		public string Name;

		public NodeEditorElementAttribute(string s)
		{
			Name = s;
		}
	}
}
