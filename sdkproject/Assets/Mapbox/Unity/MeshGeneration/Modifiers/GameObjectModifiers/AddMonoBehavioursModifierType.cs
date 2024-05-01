using System;
using UnityEngine;

namespace Mapbox.Unity.MeshGeneration.Modifiers
{
#if UNITY_EDITOR
	using UnityEditor;
#endif

	[Serializable]
	public class AddMonoBehavioursModifierType
	{
		[SerializeField] private string _typeString;

#if UNITY_EDITOR
		[SerializeField] private MonoScript _script;
#endif

		private Type _type;

		public Type Type
		{
			get
			{
				if (_type == null) _type = Type.GetType(_typeString);
				return _type;
			}
		}
	}
}
