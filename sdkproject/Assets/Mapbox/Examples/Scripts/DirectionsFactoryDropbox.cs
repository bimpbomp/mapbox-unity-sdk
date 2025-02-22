﻿using Mapbox.Directions;
using Mapbox.Unity.MeshGeneration.Factories;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class DirectionsFactoryDropbox : MonoBehaviour
{
	private Dropdown _dropdown;
	private DirectionsFactory _factory;

	private void Start()
	{
		_factory = FindObjectOfType<DirectionsFactory>();
		_dropdown = GetComponent<Dropdown>();
		if (_dropdown != null)
			_dropdown.onValueChanged.AddListener(index =>
			{
				switch (index)
				{
					case 0:
						_factory.ChangeRoutingProfile(RoutingProfile.Driving);
						break;
					case 1:
						_factory.ChangeRoutingProfile(RoutingProfile.Walking);
						break;
					case 2:
						_factory.ChangeRoutingProfile(RoutingProfile.Cycling);
						break;
				}
			});
	}
}
