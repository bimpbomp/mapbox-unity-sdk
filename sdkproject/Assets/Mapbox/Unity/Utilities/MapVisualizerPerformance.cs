using System;
using System.Diagnostics;
using Mapbox.Unity.Map;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Mapbox.Unity.Utilities
{
	public class MapVisualizerPerformance : MonoBehaviour
	{
		public int TestCount = 10;
		private int _currentTest = 1;
		private float _firstRun;
		private AbstractMap _map;
		private AbstractMapVisualizer _mapVisualizer;
		private readonly Stopwatch _sw = new();

		[NonSerialized] public float TotalTime;

		protected virtual void Awake()
		{
			TotalTime = 0;
			_currentTest = 1;
			_map = FindObjectOfType<AbstractMap>();
			_mapVisualizer = _map.MapVisualizer;

			_mapVisualizer.OnMapVisualizerStateChanged += s =>
			{
				if (s == ModuleState.Working)
				{
					_sw.Reset();
					_sw.Start();
				}
				else if (s == ModuleState.Finished)
				{
					_sw.Stop();
					if (_currentTest > 1)
					{
						TotalTime += _sw.ElapsedMilliseconds;
						Debug.Log("Test " + _currentTest + ": " + _sw.ElapsedMilliseconds);
					}
					else
					{
						_firstRun = _sw.ElapsedMilliseconds;
					}

					if (TestCount > _currentTest)
					{
						_currentTest++;
						Invoke("Run", 1f);
					}
					else
					{
						if (_currentTest > 1)
							Debug.Log("First Run:        " + _firstRun + " \r\nRest Average: " +
							          TotalTime / (_currentTest - 1));
					}
				}
			};
		}

		public void Run()
		{
			//TODO : FIX THIS ERROR	
			//_map.Reset();
		}
	}
}
