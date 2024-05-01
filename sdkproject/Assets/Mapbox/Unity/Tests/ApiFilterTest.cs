using System.Linq;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Filters;
using UnityEngine;

public class ApiFilterTest : MonoBehaviour
{
	public string layerToWorkWith;

	public string Key;
	public LayerFilterOperationType layerFilterOperationType;
	public float min;
	public float max;
	public string property;

	public LayerFilterCombinerOperationType layerFilterCombinerOperationType;
	private AbstractMap _abstractMap;

	private VectorSubLayerProperties[] _layers;

	private void Start()
	{
		_abstractMap = FindObjectOfType<AbstractMap>();
	}

	private VectorSubLayerProperties[] GetLayers()
	{
		VectorSubLayerProperties[] vectorSubLayers;
		if (!string.IsNullOrEmpty(layerToWorkWith))
			vectorSubLayers = new[] { _abstractMap.VectorData.FindFeatureSubLayerWithName(layerToWorkWith) };
		else
			vectorSubLayers = _abstractMap.VectorData.GetAllFeatureSubLayers().ToArray();
		return vectorSubLayers;
	}

	private void DebugFilterInfo(ILayerFilter layerFilter)
	{
		Debug.Log("Key : " + layerFilter.GetKey);
		Debug.Log("Operator : " + layerFilter.GetFilterOperationType);
		Debug.Log("Property : " + layerFilter.GetPropertyValue);
		Debug.Log("Min/Max : " + layerFilter.GetMinValue + ", " + layerFilter.GetMaxValue);
	}

	[ContextMenu("Check Filter ALL")]
	public void CheckFilterAll()
	{
		var layers = GetLayers();
		for (var i = 0; i < layers.Length; i++)
		{
			var filters = layers[i].Filtering.GetFiltersByQuery(x =>
				x.FilterKeyMatchesExact(Key) && x.FilterUsesOperationType(layerFilterOperationType) &&
				x.FilterNumberValueEquals(min)).ToArray();
			if (filters.Length == 0) continue;
			Debug.Log(layers[i].Key);
			for (var j = 0; j < filters.Length; j++)
			{
				var layerFilter = filters[j];
				DebugFilterInfo(layerFilter);
			}
		}
	}

	[ContextMenu("Check Filter ANY")]
	public void CheckFilterAny()
	{
		var layers = GetLayers();
		for (var i = 0; i < layers.Length; i++)
		{
			var filters = layers[i].Filtering.GetFiltersByQuery(x =>
				x.FilterKeyMatchesExact(Key) || x.FilterUsesOperationType(layerFilterOperationType) ||
				x.FilterNumberValueEquals(min)).ToArray();
			if (filters.Length == 0) continue;
			Debug.Log(layers[i].Key);
			for (var j = 0; j < filters.Length; j++)
			{
				var layerFilter = filters[j];
				DebugFilterInfo(layerFilter);
			}
		}
	}

	[ContextMenu("Check Filter Key Exact")]
	public void CheckFilterKeyExact()
	{
		var layers = GetLayers();
		for (var i = 0; i < layers.Length; i++)
		{
			var filters = layers[i].Filtering.GetFiltersByQuery(x => x.FilterKeyMatchesExact(Key)).ToArray();
			if (filters.Length == 0) continue;
			Debug.Log(layers[i].Key);
			for (var j = 0; j < filters.Length; j++)
			{
				var layerFilter = filters[j];
				DebugFilterInfo(layerFilter);
			}
		}
	}

	[ContextMenu("Check Filter Key Contains")]
	public void CheckFilterKeyContains()
	{
		var layers = GetLayers();
		for (var i = 0; i < layers.Length; i++)
		{
			var filters = layers[i].Filtering.GetFiltersByQuery(x => x.FilterKeyContains(Key)).ToArray();
			if (filters.Length == 0) continue;
			Debug.Log(layers[i].Key);
			for (var j = 0; j < filters.Length; j++)
			{
				var layerFilter = filters[j];
				DebugFilterInfo(layerFilter);
			}
		}
	}

	[ContextMenu("Check Filter Uses Operation Type")]
	public void CheckFilterUsesOperationType()
	{
		var layers = GetLayers();
		for (var i = 0; i < layers.Length; i++)
		{
			var filters = layers[i].Filtering
				.GetFiltersByQuery(x => x.FilterUsesOperationType(layerFilterOperationType)).ToArray();
			if (filters.Length == 0) continue;
			Debug.Log(layers[i].Key);
			for (var j = 0; j < filters.Length; j++)
			{
				var layerFilter = filters[j];
				DebugFilterInfo(layerFilter);
			}
		}
	}

	[ContextMenu("Check Filter Has Exact Property")]
	public void CheckFilterHasExactProperty()
	{
		var layers = GetLayers();
		for (var i = 0; i < layers.Length; i++)
		{
			var filters = layers[i].Filtering.GetFiltersByQuery(x => x.FilterPropertyMatchesExact(property)).ToArray();
			if (filters.Length == 0) continue;
			Debug.Log(layers[i].Key);
			for (var j = 0; j < filters.Length; j++)
			{
				var layerFilter = filters[j];
				DebugFilterInfo(layerFilter);
			}
		}
	}

	[ContextMenu("Check Filter Contains Property")]
	public void CheckFilterContainsProperty()
	{
		var layers = GetLayers();
		for (var i = 0; i < layers.Length; i++)
		{
			var filters = layers[i].Filtering.GetFiltersByQuery(x => x.FilterPropertyContains(property)).ToArray();
			if (filters.Length == 0) continue;
			Debug.Log(layers[i].Key);
			for (var j = 0; j < filters.Length; j++)
			{
				var layerFilter = filters[j];
				DebugFilterInfo(layerFilter);
			}
		}
	}

	[ContextMenu("Check Filter Num Is Equal")]
	public void CheckFilterNumValueIsEqual()
	{
		var layers = GetLayers();
		for (var i = 0; i < layers.Length; i++)
		{
			var filters = layers[i].Filtering.GetFiltersByQuery(x => x.FilterNumberValueEquals(min)).ToArray();
			if (filters.Length == 0) continue;
			Debug.Log(layers[i].Key);
			for (var j = 0; j < filters.Length; j++)
			{
				var layerFilter = filters[j];
				DebugFilterInfo(layerFilter);
			}
		}
	}

	[ContextMenu("Check Filter Num Is Less")]
	public void CheckFilterNumValueIsLess()
	{
		var layers = GetLayers();
		for (var i = 0; i < layers.Length; i++)
		{
			var filters = layers[i].Filtering.GetFiltersByQuery(x => x.FilterNumberValueIsLessThan(min)).ToArray();
			if (filters.Length == 0) continue;
			Debug.Log(layers[i].Key);
			for (var j = 0; j < filters.Length; j++)
			{
				var layerFilter = filters[j];
				DebugFilterInfo(layerFilter);
			}
		}
	}

	[ContextMenu("Check Filter Num Is Greater")]
	public void CheckFilterNumValueIsGreater()
	{
		var layers = GetLayers();
		for (var i = 0; i < layers.Length; i++)
		{
			var filters = layers[i].Filtering.GetFiltersByQuery(x => x.FilterNumberValueIsGreaterThan(min)).ToArray();
			if (filters.Length == 0) continue;
			Debug.Log(layers[i].Key);
			for (var j = 0; j < filters.Length; j++)
			{
				var layerFilter = filters[j];
				DebugFilterInfo(layerFilter);
			}
		}
	}

	[ContextMenu("Set String Contains")]
	public void SetStringContains()
	{
		var layers = GetLayers();
		for (var i = 0; i < layers.Length; i++)
		{
			var filters = layers[i].Filtering.GetAllFilters().ToArray();
			if (filters.Length != 0)
			{
				var layerFilter = filters[i];
				layerFilter.SetStringContains(Key, property);
			}
		}
	}

	[ContextMenu("Set Number Is Equal")]
	public void SetNumberIsEqual()
	{
		var layers = GetLayers();
		for (var i = 0; i < layers.Length; i++)
		{
			var filters = layers[i].Filtering.GetAllFilters().ToArray();
			if (filters.Length != 0)
			{
				var layerFilter = filters[i];
				layerFilter.SetNumberIsEqual(Key, min);
			}
		}
	}

	[ContextMenu("Set Number Is Less Than")]
	public void SetNumberIsLessThan()
	{
		var layers = GetLayers();
		for (var i = 0; i < layers.Length; i++)
		{
			var filters = layers[i].Filtering.GetAllFilters().ToArray();
			if (filters.Length != 0)
			{
				var layerFilter = filters[i];
				layerFilter.SetNumberIsLessThan(Key, min);
			}
		}
	}

	[ContextMenu("Set Number Is Greater Than")]
	public void SetNumberIsGreaterThan()
	{
		var layers = GetLayers();
		for (var i = 0; i < layers.Length; i++)
		{
			var filters = layers[i].Filtering.GetAllFilters().ToArray();
			if (filters.Length != 0)
			{
				var layerFilter = filters[i];
				layerFilter.SetNumberIsGreaterThan(Key, min);
			}
		}
	}

	[ContextMenu("Set Number Is In Range")]
	public void SetNumberIsInRange()
	{
		var layers = GetLayers();
		for (var i = 0; i < layers.Length; i++)
		{
			var filters = layers[i].Filtering.GetAllFilters().ToArray();
			if (filters.Length != 0)
			{
				var layerFilter = filters[i];
				layerFilter.SetNumberIsInRange(Key, min, max);
			}
		}
	}

	[ContextMenu("Add String Filter Contains")]
	public void AddStringFilterContains()
	{
		var layers = GetLayers();
		for (var i = 0; i < layers.Length; i++) layers[i].Filtering.AddStringFilterContains(Key, property);
	}

	[ContextMenu("Add Numeric Filter Equals")]
	public void AddNumericFilterEquals()
	{
		var layers = GetLayers();
		for (var i = 0; i < layers.Length; i++) layers[i].Filtering.AddNumericFilterEquals(Key, min);
	}

	[ContextMenu("Add Numeric Filter Is Less Than")]
	public void AddNumericFilterLessThan()
	{
		var layers = GetLayers();
		for (var i = 0; i < layers.Length; i++) layers[i].Filtering.AddNumericFilterLessThan(Key, min);
	}

	[ContextMenu("Add Numeric Filter Is Greater Than")]
	public void AddNumericFilterGreaterThan()
	{
		var layers = GetLayers();
		for (var i = 0; i < layers.Length; i++) layers[i].Filtering.AddNumericFilterGreaterThan(Key, min);
	}
}
