using System.Collections;
using UnityEngine;

public class BlinkLight : MonoBehaviour
{
	private readonly float blinkDuration = 2.0f;

	private void Start()
	{
		StartCoroutine(BlinkLed());
	}

	private IEnumerator BlinkLed()
	{
		var halo = gameObject.GetComponent("Halo");
		while (true)
		{
			((Behaviour)halo).enabled = !((Behaviour)halo).enabled;
			yield return new WaitForSeconds(blinkDuration);
		}
	}
}
